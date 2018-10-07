using System;

namespace Sababa.Logic.Tests.TestsSelfDIContainer.CustomTypes
{
    public class Manager : IDirector, IDisposable
    {
        private IWorker _worker;

        public Manager(IWorker worker)
        {
            _worker = worker;
        }

        public bool IsDisposed { get; set; }

        public string Command()
        {
            return $"Manager: {_worker.Work()}";
        }

        public void Dispose()
        {
            IsDisposed = true;
        }
    }
}
