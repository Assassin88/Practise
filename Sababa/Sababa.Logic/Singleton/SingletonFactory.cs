namespace Sababa.Logic.Singleton
{
    public class SingletonFactory
    {
        internal SingletonFactory()
        {

        }
    }

    public static class Factory
    {
        public static SingletonFactory GetInstance()
        {
            return SingletonInstance.GetSingletonFactoryInstance();
        }

        private static class SingletonInstance
        {
            private static readonly SingletonFactory _singletonFactory = new SingletonFactory();

            public static SingletonFactory GetSingletonFactoryInstance()
            {
                return _singletonFactory;
            }
        }
    }
}
