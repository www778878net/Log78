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
      var log = new Log78();
      var mockServerLogger = new MockServerLogger();
      var mockFileLogger = new MockFileLogger();
      var mockConsoleLogger = new MockConsoleLogger();

      log.setup(mockServerLogger, mockFileLogger, mockConsoleLogger, "testUser");

      Assert.AreEqual("testUser", log.uname, "用户名应该被正确设置");
    }

    [TestMethod]
    public void TestClone()
    {
      var originalLog = new Log78
      {
        LevelApi = 80,
        LevelConsole = 40,
        LevelFile = 60,
        uname = "originalUser"
      };

      var clonedLog = originalLog.Clone();

      Assert.AreEqual(originalLog.LevelApi, clonedLog.LevelApi, "API级别应该相同");
      Assert.AreEqual(originalLog.LevelConsole, clonedLog.LevelConsole, "控制台级别应该相同");
      Assert.AreEqual(originalLog.LevelFile, clonedLog.LevelFile, "文件级别应该相同");
      Assert.AreEqual(originalLog.uname, clonedLog.uname, "用户名应该相同");
    }

    [TestMethod]
    public void TestLogErr()
    {
      var log = new Log78();
      var mockConsoleLogger = new MockConsoleLogger();
      log.setup(null, null, mockConsoleLogger, "testUser");

      var exception = new Exception("测试异常");
      log.LogErr(exception, "testKey");

      Assert.IsTrue(mockConsoleLogger.LastLoggedMessage.Contains("测试异常"), "异常消息应该被记录");
      Assert.IsTrue(mockConsoleLogger.LastLoggedMessage.Contains("testKey"), "key1应该被记录");
    }

    [TestMethod]
    public void TestLog()
    {
      var log = new Log78();
      var mockServerLogger = new MockServerLogger();
      var mockFileLogger = new MockFileLogger();
      var mockConsoleLogger = new MockConsoleLogger();

      log.setup(mockServerLogger, mockFileLogger, mockConsoleLogger, "testUser");
      log.LevelApi = 50;
      log.LevelFile = 30;
      log.LevelConsole = 10;

      log.Log("测试消息", 40, "key1", "key2");

      Assert.IsFalse(mockServerLogger.WasLogCalled, "服务器日志不应被调用");
      Assert.IsTrue(mockFileLogger.WasLogCalled, "文件日志应被调用");
      Assert.IsTrue(mockConsoleLogger.WasLogCalled, "控制台日志应被调用");
    }
  }

  // 模拟类的实现
  public class MockServerLogger : IServerLog78
  {
    public bool WasLogCalled { get; private set; }
    public void LogToServer(string message, string key1, int level, string key2, string key3, string content, string key4, string key5, string key6)
    {
      WasLogCalled = true;
    }
  }

  public class MockFileLogger : IFileLog78
  {
    public bool WasLogCalled { get; private set; }
    public string menu { get; set; } = "test";

    public void Clear()
    {
      throw new NotImplementedException();
    }

    public void LogToFile(string info)
    {
      WasLogCalled = true;
    }
  }

  public class MockConsoleLogger : IConsoleLog78
  {
    public bool WasLogCalled { get; private set; }
    public string LastLoggedMessage { get; private set; }
    public void WriteLine(string info)
    {
      WasLogCalled = true;
      LastLoggedMessage = info;
    }
  }
}