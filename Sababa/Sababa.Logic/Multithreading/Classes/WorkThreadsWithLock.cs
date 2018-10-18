using System.Linq;

namespace Sababa.Logic.Multithreading.Classes
{
    public class WorkThreadsWithLock : BaseWorkThreads
    {
        private readonly object _writerlocker = new object();
        private readonly object _readerlocker = new object();

        public WorkThreadsWithLock(int countReaders, int countWriters, int countMessages)
            : base(countReaders, countWriters, countMessages)
        {

        }

        public override void ThreadReaders(object obj)
        {
            var index = (int) obj;
            while (!IsFinish)
            {
                lock (_readerlocker)
                {
                    if (Buffer != null)
                    {
                        ReadMessages[index].Add(Buffer);
                        Buffer = null;
                    }
                }
            }
        }

        public override void ThreadWriters(object obj)
        {
            var index = (int) obj;
            var mpt = WriteMessages.Count / Writers.Length;
            var currentThreadMessages = WriteMessages.Skip(mpt * index).Take(mpt).ToList();
            var i = 0;
            while (i < currentThreadMessages.Count)
            {
                lock (_writerlocker)
                {
                    if (Buffer == null)
                    {
                        Buffer = currentThreadMessages[i++];
                    }
                }
            }
        }

    }
}
