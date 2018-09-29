namespace Sababa.Crosscutting.Logging
{
    public interface ILogger
    {
        /// <summary>
        /// Log the specified message to the trace level
        /// </summary>
        /// <param name="message">Log message</param>
        void LogTrace(string message);

        /// <summary>
        /// Log the specified message to the debug level
        /// </summary>
        /// <param name="message">Log message</param>
        void LogDebug(string message);

        /// <summary>
        /// Log the specified message to the info level
        /// </summary>
        /// <param name="message">Log message</param>
        void LogInfo(string message);

        /// <summary>
        /// Log the specified message to the warn level
        /// </summary>
        /// <param name="message">Log message</param>
        void LogWarn(string message);

        /// <summary>
        /// Log the specified message to the error level
        /// </summary>
        /// <param name="message">Log message</param>
        void LogError(string message);

        /// <summary>
        /// Log the specified message to the fatal level
        /// </summary>
        /// <param name="message">Log message</param>
        void LogFatal(string message);
    }
}
