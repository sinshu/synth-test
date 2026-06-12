```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.8655/25H2/2025Update/HudsonValley2)
12th Gen Intel Core i7-12700K 3.60GHz, 1 CPU, 20 logical and 12 physical cores
.NET SDK 10.0.301
  [Host]     : .NET 10.0.9 (10.0.9, 10.0.926.27113), X64 RyuJIT x86-64-v3
  Job-IUYGTA : .NET 10.0.9 (10.0.9, 10.0.926.27113), X64 RyuJIT x86-64-v3

InvocationCount=1  IterationCount=10  LaunchCount=1  
UnrollFactor=1  WarmupCount=3  

```
| Method            | Mean     | Error   | StdDev  | Allocated |
|------------------ |---------:|--------:|--------:|----------:|
| CSharpSynth       | 784.1 ms | 3.36 ms | 2.22 ms |  346192 B |
| MeltySynth        | 596.7 ms | 3.02 ms | 2.00 ms |         - |
| MeltySynthEffect  | 871.3 ms | 3.68 ms | 2.19 ms |         - |
| SpessaSharp       | 483.6 ms | 3.32 ms | 1.97 ms |         - |
| SpessaSharpEffect | 740.9 ms | 4.89 ms | 3.23 ms |         - |
