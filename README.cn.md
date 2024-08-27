
<h1 align="center">Log78</h1>
<div align="center">

[English](./README.md) | 简体中文


[![License](https://img.shields.io/badge/license-Apache%202-green.svg)](https://www.apache.org/licenses/LICENSE-2.0)
[![测试状态](https://github.com/www778878net/Log78/actions/workflows/BuildandTest.yml/badge.svg?branch=main)](https://github.com/www778878net/Log78/actions/workflows/BuildandTest.yml)
[![QQ群](https://img.shields.io/badge/QQ群-323397913-blue.svg?style=flat-square&color=12b7f5&logo=qq)](https://qm.qq.com/cgi-bin/qm/qr?k=it9gUUVdBEDWiTOH21NsoRHAbE9IAzAO&jump_from=webapi&authKey=KQwSXEPwpAlzAFvanFURm0Foec9G9Dak0DmThWCexhqUFbWzlGjAFC7t0jrjdKdL)
</div>
## 反馈QQ群（点击加入）：[323397913](https://qm.qq.com/cgi-bin/qm/qr?k=it9gUUVdBEDWiTOH21NsoRHAbE9IAzAO&jump_from=webapi&authKey=KQwSXEPwpAlzAFvanFURm0Foec9G9Dak0DmThWCexhqUFbWzlGjAFC7t0jrjdKdL)

## 1. `Log78` 类文档

### 概述

`Log78` 是一个用于封装日志记录功能的类，支持多种类型的日志输出，包括控制台输出、文件输出以及服务器端输出。该类采用单例模式确保全局只有一个实例，并提供了设置不同日志级别的方法。

### 安装

通过 NuGet 包管理器安装：

dotnet add package Log78

### 使用

using www778878net.log;

var log = Log78.Instance;
log.setup(serverLogger, fileLogger, consoleLogger, "admin");
log.log("Hello, world!", 50);

### 属性

- `debugKind`: 日志调试种类列表，用于控制哪些类型的日志会被记录。
- `LevelFile`, `LevelConsole`, `LevelApi`: 分别表示文件日志、控制台日志和 API 日志的级别阈值。默认情况下，控制台日志级别为 30，文件日志级别为 50，API 日志级别为 70。
- `serverLogger`, `fileLogger`, `consoleLogger`: 分别表示服务器日志记录器、文件日志记录器和控制台日志记录器。
- `uname`: 用户名，默认为空字符串。

### 日志级别使用建议

虽然日志级别是完全可自定义的,但以下是一般使用建议:
- 0-29: 详细的调试信息,通常只在开发环境中使用
- 30-49: 一般信息,可用于跟踪应用程序的正常操作
- 50-69: 警告信息,表示潜在的问题,但不影响主要功能
- 70+: 错误和严重问题,需要立即关注

### 示例: 调整日志级别

using www778878net.log;
var log = Log78.Instance;
log.setup(serverLogger, fileLogger, consoleLogger, "admin");
// 调整控制台日志级别为0,以打印所有日志(用于调试)
log.LevelConsole = 0;
// 调整文件日志级别为60,只记录较严重的警告和错误
log.LevelFile = 60;
// 使用不同级别记录日志
log.Log("调试信息", 10); // 只会在控制台输出
log.Log("一般信息", 40); // 控制台输出,不会写入文件
log.Log("警告", 65); // 控制台和文件都会记录
log.Log("错误", 80); // 控制台、文件和API都会记录

### 方法

- `setup`: 设置日志记录器实例。
- `Clone`: 创建一个当前实例的克隆。
- `LogErr`: 记录错误日志。
- `Log`: 根据提供的参数记录日志信息。可以为每个类单独设置日志级别。

### 示例

using www778878net.log;

// 创建日志记录器实例
var serverLogger = new ServerLog78();
var fileLogger = new FileLog78("logfile");
var consoleLogger = new ConsoleLog78();

// 获取 Log78 实例
var log = Log78.Instance;

// 设置日志记录器
log.setup(serverLogger, fileLogger, consoleLogger, "admin");

// 记录一条日志
log.Log("This is a log message.", 50); // 控制台和文件都会输出，因为50 >= 30 && 50 >= 50

// 记录一条错误日志
try
{
    throw new Exception("Something went wrong!");
}
catch (Exception error)
{
    log.LogErr(error);
}

### 其他

更多详细信息，请参阅项目的 [GitHub 仓库](https://github.com/www778878net/Log78) 或 [API 文档](http://www.778878.net/docs/#/Log78/)。
