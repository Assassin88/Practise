using System.Collections.Concurrent;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using NUnit.Framework;
using Sababa.Logic.SelfCuncurrentDictionary;

namespace Sababa.Logic.Tests.TestCuncurrentDictionary
{
    [TestFixture]
    public class TestDictionary
    {
        private int _countAdd = 5000;
        private int _countUpdate = 1000;
        private int _countDelete = 500;

        [Benchmark]
        public void SyncDictionaryCheckPerformanceParallel()
        {
            SyncDictionary<string, int> _syncDictionary = new SyncDictionary<string, int>();
            Parallel.For(0, _countAdd, (i) => _syncDictionary.TryAdd(i.ToString(), i));
            Parallel.For(0, _countUpdate, (i) => _syncDictionary.TryUpdate(i.ToString(), i+1));
            Parallel.For(0, _countDelete, (i) => _syncDictionary.TryRemove(i.ToString()));
        }
        [Benchmark]
        public void SyncDictionaryCheckPerformance()
        {
            SyncDictionary<string, int> _syncDictionary = new SyncDictionary<string, int>();

            for (int count = 0; count < _countAdd; count++)
            {
                _syncDictionary.TryAdd(count.ToString(), count);
            }

            for (int count = 0; count < _countUpdate; count++)
            {
                _syncDictionary.TryUpdate(count.ToString(), count + 2);
            }

            for (int count = 0; count < _countDelete; count++)
            {
                _syncDictionary.TryRemove(count.ToString());
            }
        }

        [Benchmark]
        public void CuncDictionaryCheckPerformanceParallel()
        {
            ConcurrentDictionary<string, int> _concurrentDictionary = new ConcurrentDictionary<string, int>();
            Parallel.For(0, _countAdd, (i) => _concurrentDictionary.TryAdd(i.ToString(), i));
            Parallel.For(0, _countUpdate, (i) => _concurrentDictionary.TryUpdate(i.ToString(), i+2, i));
            Parallel.For(0, _countDelete, (i) => _concurrentDictionary.TryRemove(i.ToString(), out _));
        }

        [Benchmark]
        public void CuncDictionaryCheckPerformance()
        {
            ConcurrentDictionary<string, int> _concurrentDictionary = new ConcurrentDictionary<string, int>();

            for (int count = 0; count < _countAdd; count++)
            {
                _concurrentDictionary.TryAdd(count.ToString(), count);
            }

            for (int count = 0; count < _countUpdate; count++)
            {
                _concurrentDictionary.TryUpdate(count.ToString(), count+2, count);
            }

            for (int count = 0; count < _countDelete; count++)
            {
                _concurrentDictionary.TryRemove(count.ToString(), out _);
            }
        }


        [Test]
        public void SyncDictionaryVSConcurrentDictionary_AddUpdateDeleteOperation_CheckPerfomance()
        {
            var summary = BenchmarkRunner.Run<TestDictionary>();
        }
    }
}
