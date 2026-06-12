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
| CSharpSynth       | 791.2 ms | 3.24 ms | 2.14 ms |  346192 B |
| MeltySynth        | 600.8 ms | 2.37 ms | 1.57 ms |         - |
| MeltySynthEffect  | 865.7 ms | 2.22 ms | 1.32 ms |         - |
| SpessaSharp       | 643.1 ms | 1.74 ms | 1.03 ms |         - |
| SpessaSharpEffect | 893.8 ms | 2.86 ms | 1.89 ms |         - |
