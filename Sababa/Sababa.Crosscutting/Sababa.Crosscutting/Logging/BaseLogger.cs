using NLog;

namespace Sababa.Crosscutting.Logging
{
    public class BaseLogger : ILogger
    {
        private readonly Logger _logger;

        /// <summary>
        /// Initialize a new instance of the BaseLogger class, which has the specified logger's name
        /// </summary>
        /// <param name="loggerName">The name of the creating logger</param>
        public BaseLogger(string loggerName)
        {
            _logger = LogManager.GetLogger(loggerName);
        }

        public void LogDebug(string message)
        {
            _logger.Debug(message);
        }

        public void LogError(string message)
        {
            _logger.Error(message);
        }

        public void LogFatal(string message)
        {
            _logger.Fatal(message);
        }

        public void LogInfo(string message)
        {
            _logger.Info(message);
        }

        public void LogTrace(string message)
        {
            _logger.Trace(message);
        }

        public void LogWarn(string message)
        {
            _logger.Warn(message);
        }
    }
}
