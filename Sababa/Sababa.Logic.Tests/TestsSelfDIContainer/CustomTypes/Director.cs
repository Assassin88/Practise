using System;

namespace Sababa.Logic.Tests.TestsSelfDIContainer.CustomTypes
{
    public class Director : IDirector, IDisposable
    {
        private IWorker _worker;
        public bool IsDisposed { get; set; }
        public Director(IWorker worker)
        {
            _worker = worker;
        }

        public string Command()
        {
            return $"Director: {_worker.Work()}";
        }

        public void Dispose()
        {
            IsDisposed = true;
        }
    }
}
