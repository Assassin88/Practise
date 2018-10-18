using System.Linq;
using System.Threading;

namespace Sababa.Logic.Multithreading.Classes
{
    public class WorkThreadsWithInterlocked : BaseWorkThreads
    {
        public WorkThreadsWithInterlocked(int countReaders, int countWriters, int countMessages)
            : base(countReaders, countWriters, countMessages)
        {

        }

        public override void ThreadReaders(object obj)
        {
            var index = (int)obj;
            while (!IsFinish)
            {
                var localbuf = Buffer;
                if (localbuf != null)
                {
                    if (Interlocked.CompareExchange(ref Buffer, null, localbuf) == localbuf)
                    {
                        ReadMessages[index].Add(localbuf);
                    }
                }
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
                if (Buffer == null)
                {
                    if (Interlocked.CompareExchange(ref Buffer, currentThreadMessages[i], null) == null)
                    {
                        i++;
                    }
                }
            }
        }
    }
}
