using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using Sababa.Logic.Multithreading.Classes;

namespace Sababa.Logic.Tests.TestsMultithreading.Tests
{
    [TestOf(typeof(WorkThreadsWithAutoResetEvent))]
    class WorkThreadsWithAutoResetEventTests
    {

        [Test]
        [TestCase(3, 3, 30)]
        [TestCase(6, 3, 60)]
        [TestCase(3, 6, 30)]
        [TestCase(3, 3, 60)]
        [TestCase(6, 6, 30)]
        public void ReadWriteWithAutoResetEvent_ReadAndWriteMessagesInOtherThreads_CheckResultsOnEquals(
            int countReaders, int countWriters, int countMessages)
        {
            var threadsWithAutoReset = new WorkThreadsWithAutoResetEvent(countReaders, countWriters, countMessages);

            threadsWithAutoReset.InitThreads(ThreadPriority.Normal);
            var allReadedMessages = new List<string>();
            foreach (var messages in threadsWithAutoReset.ReadMessages)
            {
                allReadedMessages.AddRange(messages);
            }

            Assert.That(() => threadsWithAutoReset.WriteMessages, Is.EquivalentTo(allReadedMessages));
        }

    }
}
