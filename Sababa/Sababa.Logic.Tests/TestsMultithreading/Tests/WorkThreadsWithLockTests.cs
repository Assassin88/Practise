using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using Sababa.Logic.Multithreading.Classes;

namespace Sababa.Logic.Tests.TestsMultithreading.Tests
{
    [TestOf(typeof(WorkThreadsWithLock))]
    class WorkThreadsWithLockTests
    {

        [Test]
        [TestCase(3, 3, 30)]
        [TestCase(6, 3, 60)]
        [TestCase(3, 6, 30)]
        [TestCase(3, 3, 60)]
        [TestCase(6, 6, 30)]
        public void ReadWriteWithLock_ReadAndWriteMessagesInOtherThreads_CheckResultsOnEquals(
            int countReaders, int countWriters, int countMessages)
        {
            var threadsWithLock = new WorkThreadsWithLock(countReaders, countWriters, countMessages);

            threadsWithLock.InitThreads(ThreadPriority.Normal);
            var allReadedMessages = new List<string>();
            foreach (var messages in threadsWithLock.ReadMessages)
            {
                allReadedMessages.AddRange(messages);
            }

            Assert.That(() => threadsWithLock.WriteMessages, Is.EquivalentTo(allReadedMessages));
        }
    }
}
