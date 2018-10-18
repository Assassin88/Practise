using System.Threading;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using NUnit.Framework;
using Sababa.Logic.Multithreading.Classes;

namespace Sababa.Logic.Tests.TestsMultithreading.TestsBenchmark
{
    [TestOf(typeof(WorkThreadsWithAutoResetEvent))]
    public class BenchmarkTestAutoResetEvent
    {
        [Params(3)]
        public int CountReaders { get; set; }

        [Params(3, 6)]
        public int CountWriters { get; set; }

        [Params(900, 1200)]
        public int CountMessages { get; set; }

        [Params(ThreadPriority.AboveNormal, ThreadPriority.BelowNormal)]
        public ThreadPriority Priority { get; set; }

        private WorkThreadsWithAutoResetEvent _threadsWithAutoResetEvent;

        [IterationSetup]
        public void Initialize()
        {
            _threadsWithAutoResetEvent = new WorkThreadsWithAutoResetEvent(CountReaders, CountWriters, CountMessages);
        }

        [Benchmark]
        public void WorkThreadsWithAutoResetEventBenchmark()
        {
            _threadsWithAutoResetEvent.InitThreads(Priority);
        }

        [Test]
        public void AutoResetEvent_GetValues_CheckValues()
        {
            var summary = BenchmarkRunner.Run<BenchmarkTestAutoResetEvent>();
        }
    }
}
