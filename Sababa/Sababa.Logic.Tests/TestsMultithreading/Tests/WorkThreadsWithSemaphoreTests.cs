using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using Sababa.Logic.Multithreading.Classes;

namespace Sababa.Logic.Tests.TestsMultithreading.Tests
{
    [TestOf(typeof(WorkThreadsWithSemaphore))]
    class WorkThreadsWithSemaphoreTests
    {

        [Test]
        [TestCase(3, 3, 30)]
        [TestCase(6, 3, 60)]
        [TestCase(3, 6, 30)]
        [TestCase(3, 3, 60)]
        [TestCase(6, 6, 30)]
        public void ReadWriteWithSemaphore_ReadAndWriteMessagesInOtherThreads_CheckResultsOnEquals(
            int countReaders, int countWriters, int countMessages)
        {
            var threadsWithSemaphore = new WorkThreadsWithSemaphore(countReaders, countWriters, countMessages);

            threadsWithSemaphore.InitThreads(ThreadPriority.Normal);
            var allReadedMessages = new List<string>();
            foreach (var messages in threadsWithSemaphore.ReadMessages)
            {
                allReadedMessages.AddRange(messages);
            }

            Assert.That(() => threadsWithSemaphore.WriteMessages, Is.EquivalentTo(allReadedMessages));
        }

    }
}