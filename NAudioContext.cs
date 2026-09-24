using System;
using NAudio.Midi;
using NAudio.Sampler;
using NAudio.Sequencing;
using NAudio.SoundFont;

class NAudioContext : IDisposable
{
    private readonly SoundFont soundFont;
    private readonly MidiFileSequence sequence;
    private readonly bool enableEffects;
    private readonly float[] buffer;
    private SequencedMidiPlayer sequencer = null!;

    public NAudioContext(bool enableEffects)
    {
        this.enableEffects = enableEffects;
        soundFont = new SoundFont(Settings.SoundFontPath);
        sequence = MidiFileSequence.FromFile(Settings.MidiFilePath);
        buffer = new float[2 * Settings.GetBufferLength()];
        Reset();
    }

    // NAudio has no full instrument reset API. Recreate the playback state outside
    // the measured iteration so controllers, voices and effect tails cannot leak.
    public void Reset()
    {
        var sampler = new SoundFontSampler(soundFont, Settings.SampleRate, Settings.MaximumPolyphony);
        NAudioCompatibility.Configure(sampler, enableEffects);
        var transport = new Transport(sequence.TempoMap, Settings.SampleRate);
        sequencer = new SequencedMidiPlayer(transport, sequence.Timeline, sampler);
    }

    public void Test(string name)
    {
        Execute();
        Utils.Write(buffer, Settings.SampleRate, name + "1.wav");
        Reset();
        Execute();
        Utils.Write(buffer, Settings.SampleRate, name + "2.wav");
    }

    public void Execute()
    {
        sequencer.Transport.SeekFrames(0);
        sequencer.Transport.Play();
        for (var offset = 0; offset < buffer.Length; offset += 2 * Settings.BlockSize)
        {
            sequencer.Read(buffer.AsSpan(offset, 2 * Settings.BlockSize));
        }
    }

    public void Dispose()
    {
    }
}
