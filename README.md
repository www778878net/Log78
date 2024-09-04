<h1 align="center">Log78</h1>
<div align="center">


English | [简体中文](./README.cn.md) 


[![License](https://img.shields.io/badge/license-Apache%202-green.svg)](https://www.apache.org/licenses/LICENSE-2.0)
[![Test Status](https://github.com/www778878net/Log78/actions/workflows/BuildandTest.yml/badge.svg?branch=main)](https://github.com/www778878net/Log78/actions/workflows/BuildandTest.yml)
[![QQ Group](https://img.shields.io/badge/QQ%20Group-323397913-blue.svg?style=flat-square&color=12b7f5&logo=qq)](https://qm.qq.com/cgi-bin/qm/qr?k=it9gUUVdBEDWiTOH21NsoRHAbE9IAzAO&jump_from=webapi&authKey=KQwSXEPwpAlzAFvanFURm0Foec9G9Dak0DmThWCexhqUFbWzlGjAFC7t0jrjdKdL)
</div>


## Feedback QQ Group (Click to join): [323397913](https://qm.qq.com/cgi-bin/qm/qr?k=it9gUUVdBEDWiTOH21NsoRHAbE9IAzAO&jump_from=webapi&authKey=KQwSXEPwpAlzAFvanFURm0Foec9G9Dak0DmThWCexhqUFbWzlGjAFC7t0jrjdKdL)

## 1. `Log78` Class Documentation

### Overview

`Log78` is a class for encapsulating logging functionality, supporting various types of log output including console output, file output, and server-side output. This class uses the singleton pattern to ensure there is only one instance globally and provides methods for setting different log levels.

### Installation

Install via NuGet Package Manager:

```
dotnet add package Log78
```

### Usage

```csharp
using www778878net.log;

var log = Log78.Instance;
log.setup(serverLogger, fileLogger, consoleLogger);

var logEntry = new LogEntry();
logEntry.Basic.Message = "Hello, world!";
log.INFO(logEntry);
```

### Properties

- `debugKind`: A HashSet of log debugging keywords used to control which types of logs are recorded.
- `LevelFile`, `LevelConsole`, `LevelApi`: Respectively represent the threshold levels for file logs, console logs, and API logs.
- `DebugEntry`: Used to set more fine-grained debugging conditions.

### Suggested Log Levels

- DEBUG (10): Detailed debug information, typically used only in development environments
- INFO (50): General information, can be used to track normal application operations
- WARN (50): Warning information, indicating potential issues but not affecting main functionality
- ERROR (70): Errors and serious problems that require immediate attention

### Example: Adjusting Log Levels

```csharp
using www778878net.log;
var log = Log78.Instance;
log.setup(serverLogger, fileLogger, consoleLogger);
// Adjust console log level to 0 to print all logs (for debugging)
log.LevelConsole = 0;
// Adjust file log level to 60 to only record more severe warnings and errors
log.LevelFile = 60;

// Using different levels to record logs
var logEntry = new LogEntry();
logEntry.Basic.Message = "Debug information";
log.DEBUG(logEntry); // Will only output to console

logEntry.Basic.Message = "General information";
log.INFO(logEntry); // Will output to console, not recorded in file

logEntry.Basic.Message = "Warning";
log.WARN(logEntry); // Will be recorded in both console and file

logEntry.Basic.Message = "Error";
log.ERROR(logEntry); // Will be recorded in console, file, and API
```

### Methods

- `setup`: Sets up logger instances.
- `DEBUG`, `INFO`, `WARN`, `ERROR`: Record logs of different levels.
- `ERROR(Exception, LogEntry)`: Records exception error logs.

### Custom Log Entries

You can create custom log entries by inheriting from the `LogEntry` class:

```csharp
public class CustomLogEntry : LogEntry
{
    public DateTime Date { get; set; }
    public string Weather { get; set; }

    public CustomLogEntry()
    {
        Date = DateTime.Now;
        Weather = "Unknown";
        Basic.HostName = Environment.MachineName;
        Basic.UserName = Environment.UserName;
    }
}

// Using a custom log entry
var customEntry = new CustomLogEntry
{
    Basic = { Message = "Test message", Summary = "Test summary" },
    Weather = "Sunny"
};
log.INFO(customEntry);
```

### Example

```csharp
using www778878net.log;

// Create logger instances
var serverLogger = new ServerLog78();
var fileLogger = new FileLog78("logfile");
var consoleLogger = new ConsoleLog78();

// Get the Log78 instance
var log = Log78.Instance;

// Setup the logger
log.setup(serverLogger, fileLogger, consoleLogger);

// Log an information message
var infoEntry = new LogEntry();
infoEntry.Basic.Message = "This is an info message.";
log.INFO(infoEntry);

// Log an error message
try
{
    throw new Exception("Something went wrong!");
}
catch (Exception error)
{
    var errorEntry = new LogEntry();
    errorEntry.Basic.Message = "An error occurred.";
    log.ERROR(error, errorEntry);
}
```

### Other

For more detailed information, please refer to the project's [GitHub repository](https://github.com/www778878net/Log78) or the [API documentation](http://www.778878.net/docs/#/Log78/).