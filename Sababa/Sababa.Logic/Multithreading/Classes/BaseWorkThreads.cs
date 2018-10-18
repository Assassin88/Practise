using System.Collections.Generic;
using System.Threading;

namespace Sababa.Logic.Multithreading.Classes
{
    public abstract class BaseWorkThreads
    {
        protected Thread[] Readers;
        protected Thread[] Writers;
        protected string Buffer;
        protected bool IsFinish;

        public List<string> WriteMessages;
        public List<string>[] ReadMessages;

        protected BaseWorkThreads(int countReaders, int countWriters, int countMessages)
        {
            Readers = new Thread[countReaders];
            Writers = new Thread[countWriters];
            WriteMessages = new List<string>();

            ReadMessages = new List<string>[countReaders];
            for (var i = 0; i < countReaders; i++)
            {
                ReadMessages[i] = new List<string>();
            }
            
            InitMessage(countMessages);
        }

        private void InitMessage(int countMessages)
        {
            for (var i = 1; i <= countMessages; i++)
            {
                WriteMessages.Add($"Mess_{i}");
            }
        }

        public void InitThreads(ThreadPriority threadPriority)
        {
            for (var i = 0; i < Readers.Length; i++)
            {
                Readers[i] = new Thread(ThreadReaders)
                    { Name = $"RE:_{i}", Priority = threadPriority};
                Readers[i].Start(i);
            }

            for (var i = 0; i < Writers.Length; i++)
            {
                Writers[i] = new Thread(ThreadWriters)
                    { Name = $"WR:_{i}", Priority = threadPriority};
                Writers[i].Start(i);
            }

            foreach (var t in Writers)
                t.Join();

            IsFinish = true;

            foreach (var t in Readers)
                t.Join();
        }

        public abstract void ThreadReaders(object obj);

        public abstract void ThreadWriters(object obj);

    }
}
