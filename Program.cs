using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

ProcessingBenchmarks.Test();

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
    }

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
