using System;
using AudioSynthesis.Midi;
using AudioSynthesis.Sequencer;
using AudioSynthesis.Synthesis;

class CSharpSynthContext : IDisposable
{
    private MidiFile midiFile;
    private Synthesizer synthesizer;
    private MidiFileSequencer sequencer;

    private float[] buffer;
    private int blockCount;

    public CSharpSynthContext()
    {
        midiFile = new MidiFile(Settings.MidiFilePath);

        synthesizer = new Synthesizer(Settings.SampleRate, 2, Settings.BlockSize, 1);
        synthesizer.LoadBank(Settings.SoundFontPath);
        sequencer = new MidiFileSequencer(synthesizer);

        sequencer.LoadMidi(midiFile);
        sequencer.Play();

        buffer = new float[2 * Settings.GetBufferLength()];
        blockCount = Settings.GetBlockCount();
    }

    public void Test()
    {
        Execute();
        Utils.Write(buffer, Settings.SampleRate, "CSharpSynth1.wav");
        Execute();
        Utils.Write(buffer, Settings.SampleRate, "CSharpSynth2.wav");
    }

    public void Execute()
    {
        sequencer.Seek(TimeSpan.Zero);
        var pos = 0;
        for (var i = 0; i < blockCount; i++)
        {
            sequencer.FillMidiEventQueue(true);
            synthesizer.GetNext();
            synthesizer.sampleBuffer.CopyTo(buffer, pos);
            pos += synthesizer.sampleBuffer.Length;
        }
    }

    public void Dispose()
    {
    }
}
