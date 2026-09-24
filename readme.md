```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.9457/25H2/2025Update/HudsonValley2)
12th Gen Intel Core i7-12700K 3.60GHz, 1 CPU, 20 logical and 12 physical cores
.NET SDK 10.0.401
  [Host]     : .NET 10.0.12 (10.0.12, 10.0.1226.42308), X64 RyuJIT x86-64-v3
  Job-IUYGTA : .NET 10.0.12 (10.0.12, 10.0.1226.42308), X64 RyuJIT x86-64-v3

InvocationCount=1  IterationCount=10  LaunchCount=1  
UnrollFactor=1  WarmupCount=3  

```
| Method            | Mean       | Error   | StdDev  | Allocated |
|------------------ |-----------:|--------:|--------:|----------:|
| CSharpSynth       | 4,686.0 ms | 7.92 ms | 4.14 ms |  346192 B |
| MeltySynth        |   604.7 ms | 1.43 ms | 0.95 ms |         - |
| MeltySynthEffect  |   861.8 ms | 4.22 ms | 2.51 ms |         - |
| SpessaSharp       |   459.5 ms | 8.54 ms | 5.65 ms |         - |
| SpessaSharpEffect |   708.7 ms | 1.94 ms | 1.02 ms |         - |
| NAudio            | 1,455.4 ms | 9.66 ms | 5.75 ms |  223816 B |
| NAudioEffect      | 1,898.1 ms | 3.56 ms | 2.35 ms |  223816 B |
