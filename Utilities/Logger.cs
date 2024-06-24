
namespace Utilities
{
    public sealed class Logger
    {
        private static Logger instance;
        private static readonly object lockObj = new object();

        private Logger()
        {
        }

        public static Logger Instance
        {
            get
            {
                if (instance is null)
                {                
                    lock (lockObj)
                    {
                        if (instance is null)
                        {
                            instance = new Logger();
                        }
                    }
                }
                return instance;
            }
        }

        public void Log(string message)
        {
            Console.WriteLine($"{DateTime.Now}: {message}");
        }
    }
}
