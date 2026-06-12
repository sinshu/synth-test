using System;
using System.IO;
using SpessaSharp.MIDI;
using SpessaSharp.Sequencer;
using SpessaSharp.SoundBank;
using SpessaSharp.Synthesizer;
using SpessaSharp.Synthesizer.Engine.Parameters;

class SpessaSharpContext : IDisposable
{
    private readonly SpessaSharpProcessor processor;
    private readonly SpessaSharpSequencer sequencer;

    private readonly float[] left;
    private readonly float[] right;

    public SpessaSharpContext(bool enableEffects)
    {
        var options = Synthesizer.Options.Default with
        {
            MaxBufferSize = Settings.BlockSize,
            EventsEnabled = false,
            EffectsEnabled = enableEffects
        };

        processor = new SpessaSharpProcessor(Settings.SampleRate, options);
        processor.Set(GlobalSystemParameter.Of(Synthesizer.InterpolationType.Linear));
        processor.Set(GlobalSystemParameter.Of(GlobalSystemParameter.Type.VoiceCap, Settings.MaximumPolyphony));

        var soundBank = SoundBank.From(new FileInfo(Settings.SoundFontPath));
        processor.SoundBankManager.Add(soundBank, "main");

        var midi = Midi.From(new FileInfo(Settings.MidiFilePath));
        sequencer = new SpessaSharpSequencer(processor);
        sequencer.LoadNewSongList(new[] { midi });

        left = new float[Settings.GetBufferLength()];
        right = new float[Settings.GetBufferLength()];
    }

    public void Test(string name)
    {
        Execute();
        Utils.Write(left, right, Settings.SampleRate, name + "1.wav");
        Execute();
        Utils.Write(left, right, Settings.SampleRate, name + "2.wav");
    }

    public void Execute()
    {
        sequencer.CurrentTime = TimeSpan.Zero;
        sequencer.Play();

        var offset = 0;
        while (offset < left.Length)
        {
            sequencer.ProcessTick();
            processor.Process(
                new ArraySegment<float>(left, offset, Settings.BlockSize),
                new ArraySegment<float>(right, offset, Settings.BlockSize));
            offset += Settings.BlockSize;
        }
    }

    public void Dispose()
    {
        processor.DestroySynthProcessor();
    }
}
