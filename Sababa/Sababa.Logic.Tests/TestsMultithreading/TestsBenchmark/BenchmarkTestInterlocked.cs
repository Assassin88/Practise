using System.Threading;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using NUnit.Framework;
using Sababa.Logic.Multithreading.Classes;

namespace Sababa.Logic.Tests.TestsMultithreading.TestsBenchmark
{
    [TestOf(typeof(WorkThreadsWithInterlocked))]
    public class BenchmarkTestInterlocked
    {
        [Params(3)]
        public int CountReaders { get; set; }

        [Params(3, 6)]
        public int CountWriters { get; set; }

        [Params(900, 1200)]
        public int CountMessages { get; set; }

        [Params(ThreadPriority.AboveNormal, ThreadPriority.BelowNormal)]
        public ThreadPriority Priority { get; set; }

        private WorkThreadsWithInterlocked _threadsWithInterlocked;

        [IterationSetup]
        public void Initialize()
        {
            _threadsWithInterlocked = new WorkThreadsWithInterlocked(CountReaders, CountWriters, CountMessages);
        }

        [Benchmark]
        public void WorkThreadsWithInterlockedBenchmark()
        {
            _threadsWithInterlocked.InitThreads(Priority);
        }

        [Test]
        public void Interlocked_GetValues_CheckValues()
        {
            var summary = BenchmarkRunner.Run<BenchmarkTestInterlocked>();
        }
    }
}
