using System.Threading;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Sababa.Logic.Multithreading.Classes;

namespace Sababa.Logic.Tests.TestsMultithreading.TestsBenchmark
{
    [TestOf(typeof(WorkThreadsWithLock))]
    public class BenchmarkTestLock
    {
        [Params(3)]
        public int CountReaders { get; set; }

        [Params(3, 6)]
        public int CountWriters { get; set; }

        [Params(900, 1200)]
        public int CountMessages { get; set; }

        [Params(ThreadPriority.AboveNormal, ThreadPriority.BelowNormal)]
        public ThreadPriority Priority { get; set; }

        private WorkThreadsWithLock _threadsWithLock;

        [IterationSetup]
        public void Initialize()
        {
            _threadsWithLock = new WorkThreadsWithLock(CountReaders, CountWriters, CountMessages);
        }

        [Benchmark]
        public void WorkThreadsWithLockBenchmark()
        {
            _threadsWithLock.InitThreads(Priority);
        }

        [Test]
        public void Lock_GetValues_CheckValues()
        {
            var summary = BenchmarkRunner.Run<BenchmarkTestLock>();
        }
    }
}
