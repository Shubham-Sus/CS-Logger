using System.Runtime.CompilerServices;

namespace CSLogger
{
    /// <summary>
    /// Provides a simple logging functionality for writing log messages to a file.
    /// </summary>
    public static class Log
    {
        /// <summary>
        /// Specifies the type of log messages.
        /// </summary>
        private enum LogType
        {
            Info,
            Trace,
            Debug,
            Warning,
            Error,
            Fatal
        }

        private const string LOG_FILE_EXT = ".log";

        private static string? _logFilePath = null;
        private static string? _dateTimeFormat = null;
        private static bool _isInitialized = false;
        private static readonly object _lock = new object();

        /// <summary>
        /// Gets the path of the log file. Returns null if the logger has not been initialized.
        /// </summary>
        public static string? LogFilePath => _logFilePath;

        /// <summary>
        /// Initializes the logger with the specified parameters.
        /// </summary>
        /// <param name="logFileDirectory">The directory where the log file will be created.</param>
        /// <param name="logFileName">The name of the log file (default is "Log").</param>
        /// <param name="dataTimeFormat">The date and time format for log entries (default is "yyyy-MM-dd HH:mm:ss.fff").</param>
        /// <param name="clearExistingLog">Specifies whether to clear the content of the log file if it already exists. If true, the existing log file will be cleared; otherwise, new log entries will be appended to any existing content.</param>
        /// <exception cref="InvalidOperationException">Thrown if the logger is already initialized.</exception>
        public static void Start(string logFileDirectory, string logFileName = "Log", string dataTimeFormat = "yyyy-MM-dd HH:mm:ss.fff", bool clearExistingLog = true)
        {
            lock (_lock)
            {
                if (_isInitialized)
                {
                    throw new InvalidOperationException("Logger was already initialized. Re-initialization is not allowed.");
                }

                _logFilePath = Path.Combine(logFileDirectory, logFileName + LOG_FILE_EXT);
                _dateTimeFormat = dataTimeFormat;

                if (clearExistingLog)
                {
                    ClearFileContent(_logFilePath);
                }

                _isInitialized = true;

                Info("Log Created.");
            }
        }

        /// <summary>
        /// Clears the content of the specified file.
        /// </summary>
        /// <param name="filePath">The path to the file to clear.</param>
        private static void ClearFileContent(string filePath)
        {
            if (File.Exists(filePath))
            {
                using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    stream.SetLength(0);
                }
            }
        }

        /// <summary>
        /// Cleans up resources used by the logger, including releasing the semaphore and resetting the logger's state.
        /// </summary>
        public static void Dispose()
        {
            lock (_lock)
            {
                _isInitialized = false;
                _logFilePath = null;
                _dateTimeFormat = null;
            }
        }

        /// <summary>
        /// Writes a log entry with the specified log type and text.
        /// </summary>
        /// <param name="logType">The type of log entry.</param>
        /// <param name="text">The text to log.</param>
        private static void WriteLog(LogType logType, string text, string callingfilePath, string callingMethodName)
        {
            if (!_isInitialized)
            {
                throw new InvalidOperationException("Logger is not initialized.");
            }

            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            string logText = $"{DateTime.Now.ToString(_dateTimeFormat)} [{logType}] ({Path.GetFileNameWithoutExtension(callingfilePath)}.{callingMethodName}) {text}{Environment.NewLine}";

            lock (_lock)
            {
                File.AppendAllText(_logFilePath!, logText);
            }
        }

        /// <summary>
        /// Logs a Trace message.
        /// </summary>
        /// <param name="text">The trace message to log.</param>
        public static void Trace(string text, [CallerFilePath] string filePath = "", [CallerMemberName] string methodName = "") =>
            WriteLog(LogType.Trace, text, filePath, methodName);

        /// <summary>
        /// Logs an Info message.
        /// </summary>
        /// <param name="text">The info message to log.</param>
        public static void Info(string text, [CallerFilePath] string filePath = "", [CallerMemberName] string methodName = "") =>
            WriteLog(LogType.Info, text, filePath, methodName);

        /// <summary>
        /// Logs a Debug message.
        /// </summary>
        /// <param name="text">The debug message to log.</param>
        public static void Debug(string text, [CallerFilePath] string filePath = "", [CallerMemberName] string methodName = "") =>
            WriteLog(LogType.Debug, text, filePath, methodName);

        /// <summary>
        /// Logs a Warning message.
        /// </summary>
        /// <param name="text">The warning message to log.</param>
        public static void Warn(string text, [CallerFilePath] string filePath = "", [CallerMemberName] string methodName = "") =>
            WriteLog(LogType.Warning, text, filePath, methodName);

        /// <summary>
        /// Logs an Error message.
        /// </summary>
        /// <param name="text">The error message to log.</param>
        public static void Error(string text, [CallerFilePath] string filePath = "", [CallerMemberName] string methodName = "") =>
            WriteLog(LogType.Error, text, filePath, methodName);

        /// <summary>
        /// Logs a Fatal error message.
        /// </summary>
        /// <param name="text">The fatal error message to log.</param>
        public static void Fatal(string text, [CallerFilePath] string filePath = "", [CallerMemberName] string methodName = "") =>
            WriteLog(LogType.Fatal, text, filePath, methodName);
    }
}
