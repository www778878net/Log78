// Copyright 2024 frieda
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using www778878net.log;
using System;

namespace Test78
{
  [TestClass]
  public class Log78Tests
  {
    [TestMethod]
    public void TestSingleton()
    {
      var instance1 = Log78.Instance;
      var instance2 = Log78.Instance;
      Assert.AreSame(instance1, instance2, "单例模式应该返回相同的实例");
    }

    [TestMethod]
    public void TestSetup()
    {
      var log = Log78.Instance;
      var mockServerLogger = new MockServerLogger();
      var mockFileLogger = new MockFileLogger();
      var mockConsoleLogger = new MockConsoleLogger();

      log.setup(mockServerLogger, mockFileLogger, mockConsoleLogger);

      // 使用日志方法来间接测试记录器是否被正确设置
      var testEntry = new LogEntry();
      testEntry.Basic.Message = "Test setup";

      // 测试 API 日志
      log.LevelApi = 0; // 确保 API 日志会被记录
      log.INFO(testEntry);
      Assert.IsTrue(mockServerLogger.WasLogCalled, "服务器日志记录器应该被调用");

      // 测试文件日志
      log.LevelFile = 0; // 确保文件日志会被记录
      log.INFO(testEntry);
      Assert.IsTrue(mockFileLogger.WasLogCalled, "文件日志记录器应该被调用");

      // 测试控制台日志
      log.LevelConsole = 0; // 确保控制台日志会被记录
      log.INFO(testEntry);
      Assert.IsTrue(mockConsoleLogger.WasLogCalled, "控制台日志记录器应该被调用");

      // 重置日志级别
      log.LevelApi = 70;
      log.LevelFile = 50;
      log.LevelConsole = 30;
    }

    [TestMethod]
    public void TestClone()
    {
      var originalLog = Log78.Instance;
      originalLog.LevelApi = 80;
      originalLog.LevelConsole = 40;
      originalLog.LevelFile = 60;

      var clonedLog = originalLog.Clone();

      Assert.AreEqual(originalLog.LevelApi, clonedLog.LevelApi, "API级别应该相同");
      Assert.AreEqual(originalLog.LevelConsole, clonedLog.LevelConsole, "控制台级别应该相同");
      Assert.AreEqual(originalLog.LevelFile, clonedLog.LevelFile, "文件级别应该相同");
    }

    [TestMethod]
    public void TestCustomLogEntry()
    {
      var log = Log78.Instance;
      var mockConsoleLogger = new MockConsoleLogger();
      log.setup(null, null, mockConsoleLogger);

      var customEntry = new CustomLogEntry
      {
          Basic = { Message = "Test message", Summary = "Test summary" },
          Weather = "Sunny"
      };

      log.INFO(customEntry);

      Assert.IsTrue(mockConsoleLogger.WasLogCalled, "控制台日志应被调用");
      Assert.IsTrue(mockConsoleLogger.LastLoggedMessage.Contains("Test message"), "消息应该被记录");
      Assert.IsTrue(mockConsoleLogger.LastLoggedMessage.Contains("Test summary"), "摘要应该被记录");
      Assert.IsTrue(mockConsoleLogger.LastLoggedMessage.Contains("Sunny"), "天气应该被记录");
      Assert.IsTrue(mockConsoleLogger.LastLoggedMessage.Contains(Environment.MachineName), "主机名应该被记录");
      Assert.IsTrue(mockConsoleLogger.LastLoggedMessage.Contains(Environment.UserName), "用户名应该被记录");
    }

    [TestMethod]
    public void TestCustomLogEntryWithException()
    {
      var log = Log78.Instance;
      var mockConsoleLogger = new MockConsoleLogger();
      log.setup(null, null, mockConsoleLogger);

      var customEntry = new CustomLogEntry();
      var exception = new Exception("Test exception");

      log.ERROR(exception, customEntry);

      Assert.IsTrue(mockConsoleLogger.WasLogCalled, "控制台日志应被调用");
      Assert.IsTrue(mockConsoleLogger.LastLoggedMessage.Contains("Test exception"), "异常消息应该被记录");
      Assert.IsTrue(mockConsoleLogger.LastLoggedMessage.Contains(Environment.MachineName), "主机名应该被记录");
      Assert.IsTrue(mockConsoleLogger.LastLoggedMessage.Contains(Environment.UserName), "用户名应该被记录");
    }
  }

  // 模拟类的实现
  public class MockServerLogger : IServerLog78
  {
    public bool WasLogCalled { get; private set; }
    public string ServerUrl { get; set; } = "";
    public void LogToServer(LogEntry logEntry)
    {
      WasLogCalled = true;
    }
    public void SendLogFile(string menu, string logFile) { }
  }

  public class MockFileLogger : IFileLog78
  {
    public bool WasLogCalled { get; private set; }
    public string Menu { get; set; } = "test";

    public void LogToFile(LogEntry logEntry)
    {
      WasLogCalled = true;
    }
  }

  public class MockConsoleLogger : IConsoleLog78
  {
    public bool WasLogCalled { get; private set; }
    public string LastLoggedMessage { get; private set; }="";
    public void WriteLine(LogEntry logEntry)
    {
      WasLogCalled = true;
      LastLoggedMessage = logEntry.ToJson();
    }
  }
}