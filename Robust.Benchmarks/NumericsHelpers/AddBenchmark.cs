using System.Runtime.Intrinsics;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using Robust.Shared.Analyzers;

namespace Robust.Benchmarks.NumericsHelpers;

[Virtual]
[DisassemblyDiagnoser]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByMethod)]
public class AddBenchmark
{
    [Params(100, 1_000, 10_000, 100_000, 1_000_000)]
    public int N { get; set; }

    // reportedly this just adds noise
    // [Params(1,2)]
    // public int T { get; set; }

    private float[] _inputA = default!;
    private float[] _inputB = default!;
    private float[] _output = default!;

    private Vector512<float>[] _vectorInputA = default!;
    private Vector512<float>[] _vectorInputB = default!;
    private Vector512<float>[] _vectorOutput = default!;

    [GlobalSetup]
    public void Setup()
    {
        _inputA = new float[N];
        _inputB = new float[N];
        _output = new float[N];

        var vectorLength = N / Vector512<float>.Count;
        _vectorInputA = new Vector512<float>[vectorLength];
        _vectorInputB = new Vector512<float>[vectorLength];
        _vectorOutput = new Vector512<float>[vectorLength];
    }

    [Benchmark]
    public void Vector512_FloatArrayAdd()
    {
        Shared.Maths.NumericsHelpers.Add(_inputA, _inputB, _output);
    }

    [Benchmark]
    public void Vector512_PreloadedArrayAdd()
    {
        Shared.Maths.NumericsHelpers.Add(_vectorInputA, _vectorInputB, _vectorOutput);
    }
}
