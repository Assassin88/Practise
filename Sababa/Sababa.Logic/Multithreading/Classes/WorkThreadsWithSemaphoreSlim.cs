using System.Linq;
using System.Threading;

namespace Sababa.Logic.Multithreading.Classes
{
    public class WorkThreadsWithSemaphoreSlim : BaseWorkThreads
    {
        private readonly SemaphoreSlim _semaphoreSlimReader = new SemaphoreSlim(1);
        private readonly SemaphoreSlim _semaphoreSlimWriter = new SemaphoreSlim(1);

        public WorkThreadsWithSemaphoreSlim(int countReaders, int countWriters, int countMessages)
            : base(countReaders, countWriters, countMessages)
        {
;
        }

        public override void ThreadReaders(object obj)
        {
            var index = (int)obj;
            while (!IsFinish)
            {
                _semaphoreSlimReader.Wait();
                if (Buffer != null)
                {
                    ReadMessages[index].Add(Buffer);
                    Buffer = null;
                }

                _semaphoreSlimReader.Release();
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
                _semaphoreSlimWriter.Wait();
                if (Buffer == null)
                {
                    Buffer = currentThreadMessages[i++];
                }

                _semaphoreSlimWriter.Release();
            }
        }
    }
}
