namespace Sababa.Logic.Singleton
{
    public class SingletonClassic
    {
        private static SingletonClassic _instance;

        private SingletonClassic()
        {
        }

        public static SingletonClassic GetInstance()
        {
            if (_instance == null)
            {
                _instance = new SingletonClassic();
            }
            return _instance;
        }
    }
}