namespace Sababa.Logic.Singleton
{
    public class SingletonStatic
    {
        private static readonly SingletonStatic _instance = new SingletonStatic();

        static SingletonStatic()
        {
        }


        private SingletonStatic()
        {
        }


        public static SingletonStatic GetInstance()
        {
            return _instance;
        }
    }
}
