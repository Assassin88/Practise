using System.Linq;
using System.Threading;

namespace Sababa.Logic.Multithreading.Classes
{
    public class WorkThreadsWithAutoResetEvent : BaseWorkThreads
    {
        private readonly AutoResetEvent _areReader = new AutoResetEvent(true);
        private readonly AutoResetEvent _areWriter = new AutoResetEvent(true);

        public WorkThreadsWithAutoResetEvent(int countReaders, int countWriters, int countMessages)
            : base(countReaders, countWriters, countMessages)
        {
            
        }

        public override void ThreadReaders(object obj)
        {
            var index = (int)obj;
            while (!IsFinish)
            {
                _areReader.WaitOne();
                if (Buffer != null)
                {
                    ReadMessages[index].Add(Buffer);
                    Buffer = null;
                }

                _areReader.Set();
            }
        }

        public override void ThreadWriters(object obj)
        {
            var index = (int)obj;
            var mpt = WriteMessages.Count / Writers.Length;
            var currentThreadMessages = WriteMessages.Skip(mpt * index).Take(mpt).ToList();
            var i = 0;
            while (i < currentThreadMessages.Count)
            {
                _areWriter.WaitOne();
                if (Buffer == null)
                {
                    Buffer = currentThreadMessages[i++];
                }

                _areWriter.Set();
            }
        }
    }
}
