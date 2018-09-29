namespace Sababa.Logic.Singleton
{
    public class SingletonNested
    {
        private class Nested
        {
            internal static readonly SingletonNested _instance = new SingletonNested();
        }

        private SingletonNested()
        {
        }

        public static SingletonNested GetInstance()
        {
            return Nested._instance;
        }
    }
}
