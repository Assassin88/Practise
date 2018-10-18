using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using Sababa.Logic.Multithreading.Classes;

namespace Sababa.Logic.Tests.TestsMultithreading.Tests
{
    [TestOf(typeof(WorkThreadsWithSemaphoreSlim))]
    class WorkThreadsWithSemaphoreSlimTests
    {

        [Test]
        [TestCase(3, 3, 30)]
        [TestCase(6, 3, 60)]
        [TestCase(3, 6, 30)]
        [TestCase(3, 3, 60)]
        [TestCase(6, 6, 30)]
        public void ReadWriteWithSemaphoreSlim_ReadAndWriteMessagesInOtherThreads_CheckResultsOnEquals(
            int countReaders, int countWriters, int countMessages)
        {
            var threadsWithSemaphoreSlim = new WorkThreadsWithSemaphoreSlim(countReaders, countWriters, countMessages);

            threadsWithSemaphoreSlim.InitThreads(ThreadPriority.Normal);
            var allReadedMessages = new List<string>();
            foreach (var messages in threadsWithSemaphoreSlim.ReadMessages)
            {
                allReadedMessages.AddRange(messages);
            }

            Assert.That(() => threadsWithSemaphoreSlim.WriteMessages, Is.EquivalentTo(allReadedMessages));
        }

    }
}
