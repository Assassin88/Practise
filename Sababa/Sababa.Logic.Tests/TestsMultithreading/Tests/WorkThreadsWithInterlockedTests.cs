using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using Sababa.Logic.Multithreading.Classes;

namespace Sababa.Logic.Tests.TestsMultithreading.Tests
{
    [TestOf(typeof(WorkThreadsWithInterlocked))]
    class WorkThreadsWithInterlockedTests
    {

        [Test]
        [TestCase(3, 3, 30)]
        [TestCase(6, 3, 60)]
        [TestCase(3, 6, 30)]
        [TestCase(3, 3, 60)]
        [TestCase(6, 6, 30)]
        public void ReadWriteWithInterlocked_ReadAndWriteMessagesInOtherThreads_CheckResultsOnEquals(
            int countReaders, int countWriters, int countMessages)
        {
            var threadsWithInterlocked = new WorkThreadsWithInterlocked(countReaders, countWriters, countMessages);

            threadsWithInterlocked.InitThreads(ThreadPriority.Normal);
            var allReadedMessages = new List<string>();
            foreach (var messages in threadsWithInterlocked.ReadMessages)
            {
                allReadedMessages.AddRange(messages);
            }

            Assert.That(() => threadsWithInterlocked.WriteMessages, Is.EquivalentTo(allReadedMessages));
        }
    }
}
