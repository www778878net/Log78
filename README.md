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

`Log78` is a powerful and easy-to-use logging class that supports various types of log output including console, file, and server-side output. It uses the singleton pattern to ensure there's only one global instance, making it incredibly convenient to use throughout your application without explicit setup.

### Installation

Install via NuGet Package Manager:

```
dotnet add package Log78
```

### Quick Start

Log78 offers two ways to log messages: a simple method for quick logging and a more detailed method using `LogEntry` objects. Here's how to get started with the simple method:

```csharp
using www778878net.log;

// Get the Log78 instance - no setup required!
var log = Log78.Instance;

// Log a simple message
log.INFO("Hello, Log78!");

// Log with a summary and custom level
log.WARN("This is a warning", "Warning Summary", 60);
```

For more detailed logging, you can use the `LogEntry` object:

```csharp
var logEntry = new LogEntry();
logEntry.Basic.Message = "Detailed log message";
logEntry.Basic.Summary = "Log Summary";
log.INFO(logEntry);
```

Both methods are ready to use out of the box with default console and file logging.

### Advanced Configuration (Optional)

If you need custom logging behavior, you can use the `setup` method:

```csharp
// Create custom logger instances if needed
var serverLogger = new ServerLog78();
var fileLogger = new FileLog78("custom_logfile");
var consoleLogger = new ConsoleLog78();

// Setup custom loggers
log.setup(serverLogger, fileLogger, consoleLogger);
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

- `DEBUG`, `INFO`, `WARN`, `ERROR`: Record logs of different levels.
- `ERROR(Exception, LogEntry)`: Records exception error logs.

### Using the LogEntry Class

The `LogEntry` class provides structured information for detailed logging:

```csharp
var logEntry = new LogEntry();
logEntry.Basic.Summary = "User login successful";
logEntry.Basic.LogLevelNumber = 50;
logEntry.Basic.LogLevel = "INFO";
logEntry.Basic.Message = "User johndoe successfully logged into the system";
logEntry.Basic.ServiceName = "AuthService";
logEntry.Basic.UserId = "user123";
logEntry.Basic.UserName = "johndoe";

logEntry.Event.EventCategory = "authentication";
logEntry.Event.EventAction = "login";
logEntry.Event.EventOutcome = "success";

logEntry.Http.HttpRequestMethod = "POST";
logEntry.Http.HttpRequestBodyContent = "{\"username\":\"johndoe\",\"password\":\"*****\"}";
logEntry.Http.HttpResponseStatusCode = 200;
logEntry.Http.UrlOriginal = "https://api.example.com/login";

// Add custom properties
logEntry.AddProperty("customField", "customValue");

log.INFO(logEntry);
```

### Other

For more detailed information, please refer to the project's [GitHub repository](https://github.com/www778878net/Log78) or the [API documentation](http://www.778878.net/docs/#/Log78/).