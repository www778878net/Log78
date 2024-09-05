#nullable enable
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Reflection;

namespace www778878net.log
{
  public class Log78
  {
    public HashSet<string> debugKind = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
    public int LevelFile { get; set; } = 50;
    public int LevelConsole { get; set; } = 30;
    public int LevelApi { get; set; } = 70;
    private IServerLog78? serverLogger;
    private IConsoleLog78? consoleLogger = new ConsoleLog78();
    private IFileLog78? fileLogger= new FileLog78();

    // 公共的静态方法，用于获取单例实例
    private static Log78? instance;
    public static Log78 Instance
    {
      get
      {
        if (instance == null)
        {
          instance = new Log78();
          instance.setup(null, new FileLog78(), new ConsoleLog78());
        }
        return instance;
      }
    }

    public Log78 Clone()
    {
      return new Log78()
      {
        serverLogger = this.serverLogger,
        fileLogger = this.fileLogger,
        consoleLogger = this.consoleLogger,
        LevelApi = this.LevelApi,
        LevelConsole = this.LevelConsole,
        LevelFile = this.LevelFile

      };
    }

    //

    public void setup(IServerLog78? serverLogger, IFileLog78? fileLogger, IConsoleLog78? consoleLogger)
    {
      this.serverLogger = serverLogger;
      this.consoleLogger = consoleLogger;
      this.fileLogger = fileLogger;

    }

    public void DEBUG(LogEntry logEntry, int level = 10)
    {
      logEntry.Basic.LogLevel = "DEBUG";
      logEntry.Basic.LogLevelNumber = level;
      ProcessLog(logEntry);
    }

    public void INFO(LogEntry logEntry, int level = 50)
    {
      logEntry.Basic.LogLevel = "INFO";
      logEntry.Basic.LogLevelNumber = level;
      ProcessLog(logEntry);
    }

    public void WARN(LogEntry logEntry, int level = 50)
    {
      logEntry.Basic.LogLevel = "WARN";
      logEntry.Basic.LogLevelNumber = level;
      ProcessLog(logEntry);
    }

    public void ERROR(Exception error, LogEntry logEntry, int level = 70)
    {
      logEntry.Basic.LogLevel = "ERROR";
      logEntry.Basic.LogLevelNumber = level;

      // 将错误信息写入 LogEntry 的 ErrorInfo 中
      logEntry.Error.ErrorType = error.GetType().FullName;
      logEntry.Error.ErrorMessage = error.Message;
      logEntry.Error.ErrorStackTrace = error.StackTrace;

      // 如果 Basic.Summary 为空，可以使用错误消息作为摘要
      if (string.IsNullOrEmpty(logEntry.Basic.Summary))
      {
        logEntry.Basic.Summary = error.Message;
      }

      // 如果 Basic.Message 为空，可以使用更详细的错误信息
      if (string.IsNullOrEmpty(logEntry.Basic.Message))
      {
        logEntry.Basic.Message = $"{error.GetType().Name}: {error.Message}";
      }

      ProcessLog(logEntry);
    }

    public void ERROR(LogEntry logEntry, int level = 70)
    {
      logEntry.Basic.LogLevel = "ERROR";
      logEntry.Basic.LogLevelNumber = level;
      ProcessLog(logEntry);
    }

    private void ProcessLog(LogEntry? logEntry)
    {
      if (logEntry?.Basic == null)
      {
        ERROR(new LogEntry { Basic = new BasicInfo { Message = "Error: LogEntry or LogEntry.Basic is null" } });
        return;
      }

      bool isdebug = IsDebugKey(logEntry);

      if (isdebug || logEntry.Basic.LogLevelNumber >= LevelApi)
      {
        serverLogger?.LogToServer(logEntry);
      }

      if (isdebug || logEntry.Basic.LogLevelNumber >= LevelFile)
      {
        fileLogger?.LogToFile(logEntry);
      }

      if (isdebug || logEntry.Basic.LogLevelNumber >= LevelConsole)
      {
        consoleLogger?.WriteLine(logEntry);
      }
    }

    private bool IsDebugKey(LogEntry logEntry)
    {
      // 首先检查是否设置了 DebugEntry
      if (DebugEntry?.Basic != null)
      {
        // 如果设置了 DebugEntry，进行精细控制检查
        return
            (DebugEntry.Basic.ServiceName != null &&
             logEntry.Basic.ServiceName != null &&
             DebugEntry.Basic.ServiceName.Equals(logEntry.Basic.ServiceName, StringComparison.OrdinalIgnoreCase)) ||
            (DebugEntry.Basic.ServiceObj != null &&
             logEntry.Basic.ServiceObj != null &&
             DebugEntry.Basic.ServiceObj.Equals(logEntry.Basic.ServiceObj, StringComparison.OrdinalIgnoreCase)) ||
            (DebugEntry.Basic.ServiceFun != null &&
             logEntry.Basic.ServiceFun != null &&
             DebugEntry.Basic.ServiceFun.Equals(logEntry.Basic.ServiceFun, StringComparison.OrdinalIgnoreCase)) ||
            (DebugEntry.Basic.UserId != null &&
             logEntry.Basic.UserId != null &&
             DebugEntry.Basic.UserId.Equals(logEntry.Basic.UserId, StringComparison.OrdinalIgnoreCase)) ||
            (DebugEntry.Basic.UserName != null &&
             logEntry.Basic.UserName != null &&
             DebugEntry.Basic.UserName.Equals(logEntry.Basic.UserName, StringComparison.OrdinalIgnoreCase));
      }

      // 如果没有设置 DebugEntry，则检查 debugKind 集合
      string?[] keysToCheck = new[]
      {
                logEntry.Basic.ServiceName,
                logEntry.Basic.ServiceObj,
                logEntry.Basic.ServiceFun,
                logEntry.Basic.UserId,
                logEntry.Basic.UserName
            };

      foreach (var key in keysToCheck)
      {
        if (key != null && debugKind.Contains(key))
        {
          return true;
        }
      }

      return false;
    }

    public LogEntry? DebugEntry { get; set; }

    public void DEBUG(string summary, string message = "", int level = 10)
    {
        LogWithLevel("DEBUG", summary, message, level);
    }

    public void INFO(string summary, string message = "", int level = 50)
    {
        LogWithLevel("INFO", summary, message, level);
    }

    public void WARN(string summary, string message = "", int level = 50)
    {
        LogWithLevel("WARN", summary, message, level);
    }

    public void ERROR(string summary, string message = "", int level = 70)
    {
        LogWithLevel("ERROR", summary, message, level);
    }

    public void ERROR(Exception error, string summary = "", string message = "", int level = 70)
    {
        var logEntry = new LogEntry
        {
            Basic = new BasicInfo
            {
                Summary = string.IsNullOrEmpty(summary) ? error.GetType().Name : summary,
                Message = string.IsNullOrEmpty(message) ? error.Message : message,
                LogLevel = "ERROR",
                LogLevelNumber = level
            },
            Error = new ErrorInfo
            {
                ErrorType = error.GetType().FullName,
                ErrorMessage = error.Message,
                ErrorStackTrace = error.StackTrace
            }
        };

        ProcessLog(logEntry);
    }

    private void LogWithLevel(string logLevel, string summary, string message, int level)
    {
        var logEntry = new LogEntry
        {
            Basic = new BasicInfo
            {
                Summary = summary,
                Message = message,
                LogLevel = logLevel,
                LogLevelNumber = level
            }
        };

        ProcessLog(logEntry);
    }

  }
}