using System;

namespace Sababa.Logic.Singleton
{
    public class SingletonLazy
    {
        private static readonly Lazy<SingletonLazy> _instance =
            new Lazy<SingletonLazy>(() => new SingletonLazy());
        private SingletonLazy()
        {
        }

        public static SingletonLazy GetInstance()
        {
            return _instance.Value;
        }
    }
}
