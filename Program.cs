using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

ProcessingBenchmarks.Test();

if (Array.IndexOf(args, "--test-only") >= 0)
{
    return;
}

BenchmarkRunner.Run<ProcessingBenchmarks>(
    ManualConfig
        .Create(DefaultConfig.Instance)
        .AddJob(Job.Default
            .WithLaunchCount(1)
            .WithWarmupCount(3)
            .WithIterationCount(10)
            .WithInvocationCount(1)
            .WithUnrollFactor(1)));

[MemoryDiagnoser]
public class ProcessingBenchmarks
{
    private CSharpSynthContext? csharpSynthContext;
    private MeltySynthContext? meltySynthContext;
    private MeltySynthContext? meltySynthEffectContext;
    private SpessaSharpContext? spessaSharpContext;
    private SpessaSharpContext? spessaSharpEffectContext;
    private NAudioContext? naudioContext;
    private NAudioContext? naudioEffectContext;

    public static void Test()
    {
        Console.WriteLine("Testing CSharpSynth...");
        new CSharpSynthContext().Test();

        Console.WriteLine("Testing MeltySynth...");
        new MeltySynthContext(false).Test("MeltySynth");

        Console.WriteLine("Testing MeltySynthEffect...");
        new MeltySynthContext(true).Test("MeltySynthEffect");

        Console.WriteLine("Testing SpessaSharp...");
        new SpessaSharpContext(false).Test("SpessaSharp");

        Console.WriteLine("Testing SpessaSharpEffect...");
        new SpessaSharpContext(true).Test("SpessaSharpEffect");

        Console.WriteLine("Testing NAudio...");
        using var naudio = new NAudioContext(false);
        naudio.Test("NAudio");

        Console.WriteLine("Testing NAudioEffect...");
        using var naudioEffect = new NAudioContext(true);
        naudioEffect.Test("NAudioEffect");
    }

    [GlobalSetup(Target = nameof(NAudio))]
    public void SetupNAudio() => naudioContext = new NAudioContext(false);

    [IterationSetup(Target = nameof(NAudio))]
    public void ResetNAudio() => naudioContext!.Reset();

    [GlobalCleanup(Target = nameof(NAudio))]
    public void CleanupNAudio()
    {
        naudioContext?.Dispose();
        naudioContext = null;
    }

    [GlobalSetup(Target = nameof(NAudioEffect))]
    public void SetupNAudioEffect() => naudioEffectContext = new NAudioContext(true);

    [IterationSetup(Target = nameof(NAudioEffect))]
    public void ResetNAudioEffect() => naudioEffectContext!.Reset();

    [GlobalCleanup(Target = nameof(NAudioEffect))]
    public void CleanupNAudioEffect()
    {
        naudioEffectContext?.Dispose();
        naudioEffectContext = null;
    }

    [Benchmark]
    public void NAudio() => naudioContext!.Execute();

    [Benchmark]
    public void NAudioEffect() => naudioEffectContext!.Execute();

    [GlobalSetup(Target = nameof(CSharpSynth))]
    public void SetupCSharpSynth()
    {
        csharpSynthContext = new CSharpSynthContext();
    }

    [GlobalCleanup(Target = nameof(CSharpSynth))]
    public void CleanupCSharpSynth()
    {
        csharpSynthContext?.Dispose();
        csharpSynthContext = null;
    }

    [GlobalSetup(Target = nameof(MeltySynth))]
    public void SetupMeltySynth()
    {
        meltySynthContext = new MeltySynthContext(false);
    }

    [GlobalCleanup(Target = nameof(MeltySynth))]
    public void CleanupMeltySynth()
    {
        meltySynthContext?.Dispose();
        meltySynthContext = null;
    }

    [GlobalSetup(Target = nameof(MeltySynthEffect))]
    public void SetupMeltySynthEffect()
    {
        meltySynthEffectContext = new MeltySynthContext(true);
    }

    [GlobalCleanup(Target = nameof(MeltySynthEffect))]
    public void CleanupMeltySynthEffect()
    {
        meltySynthEffectContext?.Dispose();
        meltySynthEffectContext = null;
    }

    [GlobalSetup(Target = nameof(SpessaSharp))]
    public void SetupSpessaSharp()
    {
        spessaSharpContext = new SpessaSharpContext(false);
    }

    [GlobalCleanup(Target = nameof(SpessaSharp))]
    public void CleanupSpessaSharp()
    {
        spessaSharpContext?.Dispose();
        spessaSharpContext = null;
    }

    [GlobalSetup(Target = nameof(SpessaSharpEffect))]
    public void SetupSpessaSharpEffect()
    {
        spessaSharpEffectContext = new SpessaSharpContext(true);
    }

    [GlobalCleanup(Target = nameof(SpessaSharpEffect))]
    public void CleanupSpessaSharpEffect()
    {
        spessaSharpEffectContext?.Dispose();
        spessaSharpEffectContext = null;
    }

    [Benchmark]
    public void CSharpSynth()
    {
        csharpSynthContext!.Execute();
    }

    [Benchmark]
    public void MeltySynth()
    {
        meltySynthContext!.Execute();
    }

    [Benchmark]
    public void MeltySynthEffect()
    {
        meltySynthEffectContext!.Execute();
    }

    [Benchmark]
    public void SpessaSharp()
    {
        spessaSharpContext!.Execute();
    }

    [Benchmark]
    public void SpessaSharpEffect()
    {
        spessaSharpEffectContext!.Execute();
    }
}
