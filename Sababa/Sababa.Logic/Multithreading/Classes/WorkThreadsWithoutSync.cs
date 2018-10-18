using System.Linq;

namespace Sababa.Logic.Multithreading.Classes
{
    public class WorkThreadsWithoutSync : BaseWorkThreads
    {
        public WorkThreadsWithoutSync(int countReaders, int countWriters, int countMessages)
            : base(countReaders, countWriters, countMessages)
        {

        }

        public override void ThreadReaders(object obj)
        {
            var index = (int)obj;
            while (!IsFinish)
            {
                if (Buffer != null)
                {
                    ReadMessages[index].Add(Buffer);
                    Buffer = null;
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
                    Buffer = currentThreadMessages[i++];
                }
            }
        }

    }
}
