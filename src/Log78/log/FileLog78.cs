using System;
using System.IO;
using Serilog;
using Serilog.Core;

namespace www778878net.log
{
    public class FileLog78 : IFileLog78, IDisposable
    {
        public string Menu { get; set; }
        private Logger _logger;

        public FileLog78(string menu = "logs",string filename="7788_.log")
        {
            Menu = menu;
            string logFileName = Path.Combine(menu, filename);
            _logger = new LoggerConfiguration()
                .WriteTo.File(logFileName, rollingInterval: RollingInterval.Hour)
                .CreateLogger();
            ConfigureLogger();
        }

        private void ConfigureLogger()
        {
            string logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Menu);
            Directory.CreateDirectory(logDirectory);

            
        }

        public void LogToFile(LogEntry logEntry)
        {
            _logger.Information(logEntry.ToJson());
        }

        public void Dispose()
        {
            _logger?.Dispose();
        }
    }
}