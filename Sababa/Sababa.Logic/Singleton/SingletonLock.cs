namespace Sababa.Logic.Singleton
{
    public class SingletonLock
    {
        private static SingletonLock _instance;
        private static readonly object _locker = new object();
        private SingletonLock()
        {
        }

        public static SingletonLock GetInstance()
        {
            lock (_locker)
            {
                if (_instance == null)
                {
                    _instance = new SingletonLock();
                }
                return _instance;
            }
        }
    }
}
