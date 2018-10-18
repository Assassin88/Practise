using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using Sababa.Logic.Multithreading.Classes;

namespace Sababa.Logic.Tests.TestsMultithreading.Tests
{
    [TestOf(typeof(WorkThreadsWithoutSync))]
    class WorkThreadsTests
    {

        [Test]
        [TestCase(3, 3, 30)]
        [TestCase(6, 3, 60)]
        [TestCase(3, 6, 30)]
        [TestCase(3, 3, 60)]
        [TestCase(6, 6, 30)]
        public void ReadWriteWithoutSync_ReadAndWriteMessagesInOtherThreads_CheckResultsOnNotEquals(
            int countReaders, int countWriters, int countMessages)
        {
            var workThreadsWithoutSync = new WorkThreadsWithoutSync(countReaders, countWriters, countMessages);

            workThreadsWithoutSync.InitThreads(ThreadPriority.Normal);
            var allReadedMessages = new List<string>();
            foreach (var messages in workThreadsWithoutSync.ReadMessages)
            {
                allReadedMessages.AddRange(messages);
            }

            Assert.That(() => workThreadsWithoutSync.WriteMessages, Is.Not.EquivalentTo(allReadedMessages));
        }
    }
}
