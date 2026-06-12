using System;

internal class Settings
{
    public const string SoundFontPath = "TimGM6mb.sf2";
    public const string MidiFilePath = "flourish.mid";
    public const int SampleRate = 44100;
    public const int BlockSize = 64;
    public const int MaximumPolyphony = 256;

    private static int blockCount = 0;
    private static int bufferLength = 0;

    public static int GetBlockCount()
    {
        if (blockCount == 0)
        {
            var midiFile = new MeltySynth.MidiFile(MidiFilePath);
            var bufferLength = (int)(SampleRate * midiFile.Length.TotalSeconds);
            blockCount = bufferLength / BlockSize;
        }

        return blockCount;
    }

    public static int GetBufferLength()
    {
        return BlockSize * GetBlockCount();
    }
}
