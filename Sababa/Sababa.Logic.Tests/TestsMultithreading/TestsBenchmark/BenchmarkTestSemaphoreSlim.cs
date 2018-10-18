using System.Threading;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using NUnit.Framework;
using Sababa.Logic.Multithreading.Classes;

namespace Sababa.Logic.Tests.TestsMultithreading.TestsBenchmark
{
    [TestOf(typeof(WorkThreadsWithSemaphoreSlim))]
    public class BenchmarkTestSemaphoreSlim
    {
        [Params(3)]
        public int CountReaders { get; set; }

        [Params(3, 6)]
        public int CountWriters { get; set; }

        [Params(900, 1200)]
        public int CountMessages { get; set; }

        [Params(ThreadPriority.AboveNormal, ThreadPriority.BelowNormal)]
        public ThreadPriority Priority { get; set; }

        private WorkThreadsWithSemaphoreSlim _threadsWithSemaphoreSlim;

        [IterationSetup]
        public void Initialize()
        {
            _threadsWithSemaphoreSlim = new WorkThreadsWithSemaphoreSlim(CountReaders, CountWriters, CountMessages);
        }

        [Benchmark]
        public void WorkThreadsWithSemaphoreSlimBenchmark()
        {
            _threadsWithSemaphoreSlim.InitThreads(Priority);
        }

        [Test]
        public void SemaphoreSlim_GetValues_CheckValues()
        {
            var summary = BenchmarkRunner.Run<BenchmarkTestSemaphoreSlim>();
        }
    }
}
