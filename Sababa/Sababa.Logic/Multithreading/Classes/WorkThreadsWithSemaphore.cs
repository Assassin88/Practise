using System.Linq;
using System.Threading;

namespace Sababa.Logic.Multithreading.Classes
{
    public class WorkThreadsWithSemaphore : BaseWorkThreads
    {
        private readonly Semaphore _semaphoreReader = new Semaphore(1, 1);
        private readonly Semaphore _semaphoreWriter = new Semaphore(1, 1);

        public WorkThreadsWithSemaphore(int countReaders, int countWriters, int countMessages)
            : base(countReaders, countWriters, countMessages)
        {

        }
        
        public override void ThreadReaders(object obj)
        {
            var index = (int)obj;
            while (!IsFinish)
            {
                _semaphoreReader.WaitOne();
                if (Buffer != null)
                {
                    ReadMessages[index].Add(Buffer);
                    Buffer = null;
                }

                _semaphoreReader.Release();
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
                _semaphoreWriter.WaitOne();
                if (Buffer == null)
                {
                    Buffer = currentThreadMessages[i++];
                }

                _semaphoreWriter.Release();
            }
        }

    }
}
