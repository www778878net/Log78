#nullable enable
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;

namespace www778878net.log
{
  public class Log78
  {
    private HashSet<string> debugKind = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
    private int LevelFile { get; set; } = 30;
    private int LevelConsole { get; set; } = 60;
    private int LevelApi { get; set; } = 50;
    private IServerLog78? serverLogger;
    private IConsoleLog78? consoleLogger = new ConsoleLog78();
    private IFileLog78? fileLogger = new FileLog78();
    private IFileLog78? debugFileLogger;

    public LogEntry? DebugEntry { get; set; }
    public Environment CurrentEnvironment { get; private set; } = Environment.Production;

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

    public Log78()
    {
      SetEnvironmentFromEnvVar();
    }

    private void SetEnvironmentFromEnvVar()
    {
      string? envVar = System.Environment.GetEnvironmentVariable("LOG78_ENVIRONMENT");
      if (Enum.TryParse(envVar, true, out Environment env))
      {
        SetEnvironment(env);
      }
      else
      {
        SetEnvironment(Environment.Production);
      }
    }

    public void SetEnvironment(Environment env)
    {
      CurrentEnvironment = env;
      UpdateLogLevels();
      SetupDebugFileLogger();
    }

    private void UpdateLogLevels()
    {
      switch (CurrentEnvironment)
      {
        case Environment.Production:
          LevelConsole = 60; // ERROR
          LevelFile = 30;    // INFO
          LevelApi = 50;     // WARN
          break;
        case Environment.Development:
          LevelConsole = 20; // DEBUG
          LevelFile = 20;    // DEBUG
          LevelApi = 50;     // WARN
          break;
        case Environment.Testing:
          LevelConsole = 60; // ERROR
          LevelFile = 20;    // DEBUG
          LevelApi = 50;     // WARN
          break;
      }
    }

    private void SetupDebugFileLogger()
    {
      if (CurrentEnvironment == Environment.Development)
      {
        debugFileLogger = new FileLogDetail();
      }
      else
      {
        debugFileLogger = null;
      }
    }

    public void setup(IServerLog78? serverLogger, IFileLog78? fileLogger, IConsoleLog78? consoleLogger)
    {
      this.serverLogger = serverLogger;
      this.fileLogger = fileLogger ?? this.fileLogger;
      this.consoleLogger = consoleLogger ?? this.consoleLogger;
    }

    public void SetupDetailFile()
    {
      if (CurrentEnvironment == Environment.Development)
      {
        debugFileLogger = new FileLogDetail();
      }
      else
      {
        debugFileLogger = null;
      }
    }

    public void ClearDetailLog()
    {
      debugFileLogger?.Clear();
    }

    private async Task ProcessLogInternal(LogEntry? logEntry)
    {
      if (logEntry?.Basic == null)
      {
        await ERROR(new LogEntry { Basic = new BasicInfo { Message = "Error: LogEntry or LogEntry.Basic is null" } });
        return;
      }

      bool isdebug = IsDebugKey(logEntry);

      if (isdebug || logEntry.Basic.LogLevelNumber >= LevelApi)
      {
        if (serverLogger != null)
        {
          try
          {
            await serverLogger.LogToServer(logEntry);
          }
          catch (Exception ex)
          {
            Console.WriteLine($"Error in server logging: {ex.Message}");
          }
        }
      }

      if (CurrentEnvironment == Environment.Development && debugFileLogger != null)
      {
        debugFileLogger.LogToFile(logEntry);
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
      if (DebugEntry?.Basic != null)
      {
        return (DebugEntry.Basic.ServiceName != null &&
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

      string?[] keysToCheck = new[]
      {
        logEntry.Basic.ServiceName,
        logEntry.Basic.ServiceObj,
        logEntry.Basic.ServiceFun,
        logEntry.Basic.UserId,
        logEntry.Basic.UserName
      };

      return keysToCheck.Any(key => key != null && debugKind.Contains(key.ToLower()));
    }

    public async Task DETAIL(LogEntry logEntry, int level = 10)
    {
      logEntry.Basic.LogLevel = "DETAIL";
      logEntry.Basic.LogLevelNumber = level;
      await ProcessLogInternal(logEntry);
    }

    public async Task DEBUG(LogEntry logEntry, int level = 20)
    {
      logEntry.Basic.LogLevel = "DEBUG";
      logEntry.Basic.LogLevelNumber = level;
      await ProcessLogInternal(logEntry);
    }

    public async Task INFO(LogEntry logEntry, int level = 30)
    {
      logEntry.Basic.LogLevel = "INFO";
      logEntry.Basic.LogLevelNumber = level;
      await ProcessLogInternal(logEntry);
    }

    public async Task WARN(LogEntry logEntry, int level = 50)
    {
      logEntry.Basic.LogLevel = "WARN";
      logEntry.Basic.LogLevelNumber = level;
      await ProcessLogInternal(logEntry);
    }

    public async Task ERROR(LogEntry logEntry, int level = 60)
    {
      logEntry.Basic.LogLevel = "ERROR";
      logEntry.Basic.LogLevelNumber = level;
      await ProcessLogInternal(logEntry);
    }

    public async Task ERROR(Exception error, LogEntry logEntry, int level = 60)
    {
      logEntry.Basic.LogLevel = "ERROR";
      logEntry.Basic.LogLevelNumber = level;

      logEntry.Error.ErrorType = error.GetType().FullName;
      logEntry.Error.ErrorMessage = error.Message;
      logEntry.Error.ErrorStackTrace = error.StackTrace;

      if (string.IsNullOrEmpty(logEntry.Basic.Summary))
      {
        logEntry.Basic.Summary = error.Message;
      }

      if (string.IsNullOrEmpty(logEntry.Basic.Message))
      {
        logEntry.Basic.Message = $"{error.GetType().Name}: {error.Message}";
      }

      await ProcessLogInternal(logEntry);
    }

    public void SetupLevel(int fileLevel, int consoleLevel, int apiLevel)
    {
      LevelFile = fileLevel;
      LevelConsole = consoleLevel;
      LevelApi = apiLevel;
    }

    public (int FileLevel, int ConsoleLevel, int ApiLevel) GetCurrentLevels()
    {
      return (LevelFile, LevelConsole, LevelApi);
    }

    public Log78 Clone()
    {
        var cloned = new Log78();
        cloned.serverLogger = this.serverLogger;
        cloned.fileLogger = this.fileLogger;
        cloned.consoleLogger = this.consoleLogger;
        cloned.LevelApi = this.LevelApi;
        cloned.LevelConsole = this.LevelConsole;
        cloned.LevelFile = this.LevelFile;
        cloned.CurrentEnvironment = this.CurrentEnvironment;
    
        return cloned;
    }

    public enum Environment
    {
      Production,
      Development,
      Testing
    }
  }
}