using System;
using System.IO;
using Serilog;
using Serilog.Core;

namespace www778878net.log
{
    public class FileLogDetail : IFileLog78, IDisposable
    {
        private readonly string _logFileName;
        private readonly Logger _logger;

        public FileLogDetail()
        {
            string logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
            Directory.CreateDirectory(logDirectory);
            _logFileName = Path.Combine(logDirectory, "detail.log");

            // 清空文件
            File.WriteAllText(_logFileName, string.Empty);

            _logger = new LoggerConfiguration()
                .WriteTo.File(_logFileName, rollingInterval: RollingInterval.Infinite)
                .CreateLogger();

            Console.WriteLine($"Detail log file created at: {_logFileName}");
        }

        public void LogToFile(LogEntry logEntry)
        {
            _logger.Information(logEntry.ToJson());
            Console.WriteLine($"Logged to detail file: {_logFileName}");
        }

        public void Dispose()
        {
            _logger?.Dispose();
        }
    }
}