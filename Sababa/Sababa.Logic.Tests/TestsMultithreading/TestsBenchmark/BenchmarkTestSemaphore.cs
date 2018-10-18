using System.Threading;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using NUnit.Framework;
using Sababa.Logic.Multithreading.Classes;

namespace Sababa.Logic.Tests.TestsMultithreading.TestsBenchmark
{
    [TestOf(typeof(WorkThreadsWithSemaphore))]
    public class BenchmarkTestSemaphore
    {
        [Params(3)]
        public int CountReaders { get; set; }

        [Params(3, 6)]
        public int CountWriters { get; set; }

        [Params(900, 1200)]
        public int CountMessages { get; set; }

        [Params(ThreadPriority.AboveNormal, ThreadPriority.BelowNormal)]
        public ThreadPriority Priority { get; set; }

        private WorkThreadsWithSemaphore _threadsWithSemaphore;

        [IterationSetup]
        public void Initialize()
        {
            _threadsWithSemaphore = new WorkThreadsWithSemaphore(CountReaders, CountWriters, CountMessages);
        }

        [Benchmark]
        public void WorkThreadsWithSemaphoreBenchmark()
        {
            _threadsWithSemaphore.InitThreads(Priority);
        }

        [Test]
        public void Semaphore_GetValues_CheckValues()
        {
            var summary = BenchmarkRunner.Run<BenchmarkTestSemaphore>();
        }
    }
}
