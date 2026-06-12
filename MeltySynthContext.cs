using System;
using MeltySynth;

class MeltySynthContext : IDisposable
{
    private MidiFile midiFile;
    private MidiFileSequencer sequencer;

    private float[] left;
    private float[] right;

    public MeltySynthContext()
    {
        var settings = new SynthesizerSettings(Settings.SampleRate);
        settings.BlockSize = Settings.BlockSize;
        settings.MaximumPolyphony = Settings.MaximumPolyphony;
        settings.EnableReverbAndChorus = false;

        var soundFont = new SoundFont(Settings.SoundFontPath);
        var synthesizer = new Synthesizer(soundFont, settings);

        midiFile = new MidiFile(Settings.MidiFilePath);
        sequencer = new MidiFileSequencer(synthesizer);

        left = new float[Settings.GetBufferLength()];
        right = new float[Settings.GetBufferLength()];
    }

    public void Test()
    {
        Execute();
        Utils.Write(left, right, Settings.SampleRate, "MeltySynth1.wav");
        Execute();
        Utils.Write(left, right, Settings.SampleRate, "MeltySynth2.wav");
    }

    public void Execute()
    {
        sequencer.Play(midiFile, false);
        sequencer.Render(left, right);
    }

    public void Dispose()
    {
    }
}
