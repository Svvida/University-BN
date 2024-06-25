
using Microsoft.Extensions.Logging;

namespace Utilities
{
    public sealed class Logger
    {
        private static Logger _instance;
        private static readonly object lockObj = new object();
        private static ILogger<Logger> _logger;

        private Logger()
        {
        }

        public static Logger Instance
        {
            get
            {
                if (_instance is null)
                {                
                    lock (lockObj)
                    {
                        if (_instance is null)
                        {
                            _instance = new Logger();
                        }
                    }
                }
                return _instance;
            }
        }

        public void SetLogger(ILogger<Logger> logger)
        {
            _logger = logger;
        }

        public void Log(string message)
        {
            _logger.LogInformation(message);
        }
    }
}
