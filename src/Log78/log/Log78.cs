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
    private IFileLog78? fileLogger;

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

    public void DEBUG(LogEntry logEntry)
    {
      logEntry.Basic.LogLevel = "DEBUG";
      ProcessLog(logEntry, 10); // 假设 DEBUG 级别为 10
    }

    public void INFO(LogEntry logEntry)
    {
      logEntry.Basic.LogLevel = "INFO";
      ProcessLog(logEntry, 50); // 假设 INFO 级别为 20
    }

    public void WARN(LogEntry logEntry)
    {
      logEntry.Basic.LogLevel = "WARN";
      ProcessLog(logEntry, 50); // 假设 WARN 级别为 30
    }

    public void ERROR(Exception error, LogEntry logEntry)
    {
      logEntry.Basic.LogLevel = "ERROR";
      logEntry.Basic.LogLevelNumber = 70; // 使用 ERROR 的日志级别

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

      ProcessLog(logEntry, 70);
    }

    public void ERROR(LogEntry logEntry)
    {
      logEntry.Basic.LogLevel = "ERROR";
      ProcessLog(logEntry, 70); // 假设 ERROR 级别为 40
    }

    private void ProcessLog(LogEntry logEntry, int level)
    {
      bool isdebug = IsDebugKey(logEntry);

      if (isdebug || level >= LevelApi)
      {
        serverLogger?.LogToServer(logEntry);
      }

      if (isdebug || level >= LevelFile)
      {
        fileLogger?.LogToFile(logEntry);
      }

      if (isdebug || level >= LevelConsole)
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
            (!string.IsNullOrEmpty(DebugEntry.Basic.ServiceName) &&
             DebugEntry.Basic.ServiceName.Equals(logEntry.Basic.ServiceName, StringComparison.OrdinalIgnoreCase)) ||
            (!string.IsNullOrEmpty(DebugEntry.Basic.ServiceObj) &&
             DebugEntry.Basic.ServiceObj.Equals(logEntry.Basic.ServiceObj, StringComparison.OrdinalIgnoreCase)) ||
            (!string.IsNullOrEmpty(DebugEntry.Basic.ServiceFun) &&
             DebugEntry.Basic.ServiceFun.Equals(logEntry.Basic.ServiceFun, StringComparison.OrdinalIgnoreCase)) ||
            (!string.IsNullOrEmpty(DebugEntry.Basic.UserId) &&
             DebugEntry.Basic.UserId.Equals(logEntry.Basic.UserId, StringComparison.OrdinalIgnoreCase)) ||
            (!string.IsNullOrEmpty(DebugEntry.Basic.UserName) &&
             DebugEntry.Basic.UserName.Equals(logEntry.Basic.UserName, StringComparison.OrdinalIgnoreCase));
      }

      // 如果没有设置 DebugEntry，则检查 debugKind 集合
      string[] keysToCheck = new[]
      {
                logEntry.Basic.ServiceName,
                logEntry.Basic.ServiceObj,
                logEntry.Basic.ServiceFun,
                logEntry.Basic.UserId,
                logEntry.Basic.UserName
            };

      foreach (var key in keysToCheck)
      {
        if (!string.IsNullOrEmpty(key) && debugKind.Contains(key))
        {
          return true;
        }
      }

      return false;
    }

    public LogEntry? DebugEntry { get; set; }
  }
}