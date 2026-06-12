using System;
using System.Linq;
using NAudio.Wave;

static class Utils
{
    public static void Write(float[] left, float[] right, int sampleRate, string path)
    {
        var leftMax = left.Max(x => Math.Abs(x));
        var rightMax = right.Max(x => Math.Abs(x));
        var a = 0.99F / Math.Max(leftMax, rightMax);

        var format = new WaveFormat(sampleRate, 16, 2);
        using (var writer = new WaveFileWriter(path, format))
        {
            for (var t = 0; t < left.Length; t++)
            {
                writer.WriteSample(a * left[t]);
                writer.WriteSample(a * right[t]);
            }
        }
    }

    public static void Write(float[] buffer, int sampleRate, string path)
    {
        var max = buffer.Max(x => Math.Abs(x));
        var a = 0.99F / max;

        var format = new WaveFormat(sampleRate, 16, 2);
        using (var writer = new WaveFileWriter(path, format))
        {
            for (var t = 0; t < buffer.Length; t++)
            {
                writer.WriteSample(a * buffer[t]);
            }
        }
    }
}
