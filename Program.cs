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
    private MeltySynthEffectContext? meltySynthEffectContext;

    public static void Test()
    {
        Console.WriteLine("Testing CSharpSynth...");
        new CSharpSynthContext().Test();

        Console.WriteLine("Testing MeltySynth...");
        new MeltySynthContext().Test();

        Console.WriteLine("Testing MeltySynthEffect...");
        new MeltySynthEffectContext().Test();
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
        meltySynthContext = new MeltySynthContext();
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
        meltySynthEffectContext = new MeltySynthEffectContext();
    }

    [GlobalCleanup(Target = nameof(MeltySynthEffectContext))]
    public void CleanupMeltySynthEffect()
    {
        meltySynthEffectContext?.Dispose();
        meltySynthEffectContext = null;
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
}
