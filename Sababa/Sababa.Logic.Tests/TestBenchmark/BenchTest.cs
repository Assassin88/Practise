using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using NUnit.Framework;

namespace Sababa.Logic.Tests.TestBenchmark
{
    [TestFixture]
    public class BenchTest
    {
        [Benchmark]
        public int GetValue() => new HashObject().GetHashCode();

        [Benchmark]
        public int GetValue2() => new HashObject2().GetHashCode();


        [Benchmark]
        public int GetSum() => new HashObject().SumBytes("Hello world!!!");

        [Benchmark]
        public int GetSum2() => new HashObject2().SumBytes("Hello_world");

        [Test]
        public void GetHashCodeValues_GetValues_CheckValues()
        {
            var summary = BenchmarkRunner.Run<BenchTest>();
        }

    }
}
