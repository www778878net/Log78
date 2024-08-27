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

dotnet add package Log78

### Usage
```c#
using www778878net.log;

var log = Log78.Instance;
log.setup(serverLogger, fileLogger, consoleLogger, "admin");
log.log("Hello, world!", 50);
```
### Properties

- `debugKind`: A list of log debugging kinds used to control which types of logs are recorded.
- `LevelFile`, `LevelConsole`, `LevelApi`: Respectively represent the threshold levels for file logs, console logs, and API logs. By default, the console log level is 30, the file log level is 50, and the API log level is 70.
- `serverLogger`, `fileLogger`, `consoleLogger`: Respectively represent the server logger, file logger, and console logger instances.
- `uname`: The username, which defaults to an empty string.

### Methods

- `setup`: Sets up the logger instances.
- `Clone`: Creates a clone of the current instance.
- `LogErr`: Logs error messages.
- `Log`: Logs messages based on the provided parameters. Log levels can be set individually for each class.

### Example
```c#
using www778878net.log;

// Create logger instances
var serverLogger = new ServerLog78();
var fileLogger = new FileLog78("logfile");
var consoleLogger = new ConsoleLog78();

// Get the Log78 instance
var log = Log78.Instance;

// Setup the logger
log.setup(serverLogger, fileLogger, consoleLogger, "admin");

// Log a message
log.Log("This is a log message.", 50); // Both console and file will output because 50 >= 30 && 50 >= 50

// Log an error message
try
{
    throw new Exception("Something went wrong!");
}
catch (Exception error)
{
    log.LogErr(error);
}
```
### Other

For more detailed information, please refer to the project's [GitHub repository](https://github.com/www778878net/Log78) or the [API documentation](http://www.778878.net/docs/#/Log78/).