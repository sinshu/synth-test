using System;
using System.Reflection;
using NAudio.Dsp;
using NAudio.Effects;
using NAudio.Sampler;
using NAudio.Wave;

// Compatibility with the pinned NAudio.Sampler 3.1.0 implementation:
// no public interpolation selector or true effect-disable switch is available.
// All reflection happens during setup, never during measured rendering.
internal static class NAudioCompatibility
{
    public static void Configure(SoundFontSampler sampler, bool enableEffects)
    {
        var engineType = typeof(SamplerEngine);
        var voices = (Array)GetField(engineType, "voices").GetValue(sampler)!;
        var voiceType = voices.GetType().GetElementType()!;
        var reader = GetField(voiceType, "reader");
        var readerRight = GetField(voiceType, "readerRight");
        var placeholder = new SampleSource(new float[1], Settings.SampleRate);
        foreach (var voice in voices)
        {
            // Start() reuses these readers via Reset(source), preserving Quality.
            reader.SetValue(voice, new InterpolatingSampleReader(placeholder) { Quality = InterpolationQuality.Linear });
            readerRight.SetValue(voice, new InterpolatingSampleReader(placeholder) { Quality = InterpolationQuality.Linear });
        }

        if (!enableEffects)
        {
            // AudioEffect.Bypass still runs DSP and returns dry input, which a
            // send bus adds back into the mix. Replace only the two return effects
            // with silence instead; the sampler's send/mix plumbing is retained.
            DisableBus(sampler, "reverbBus");
            DisableBus(sampler, "chorusBus");
        }
    }

    private static void DisableBus(SoundFontSampler sampler, string name)
    {
        var bus = new SendBus(new SilentEffect());
        bus.Configure(sampler.WaveFormat, Settings.BlockSize);
        GetField(typeof(SamplerEngine), name).SetValue(sampler, bus);
    }

    private static FieldInfo GetField(Type type, string name)
    {
        return type.GetField(name, BindingFlags.Instance | BindingFlags.NonPublic)
            ?? throw new NotSupportedException($"NAudio internals changed: {type.FullName}.{name}. Review NAudioCompatibility before benchmarking.");
    }

    private sealed class SilentEffect : AudioEffect
    {
        protected override void OnConfigure(WaveFormat format) { }
        protected override void ProcessBlock(Span<float> buffer) => buffer.Clear();
    }
}
