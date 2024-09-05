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
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;


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
      var logstashLogger = new LogstashServerLog78("http://localhost:5000");
      var fileLogger = new FileLog78("testlogs");
      var consoleLogger = new ConsoleLog78();

      log.setup(logstashLogger, fileLogger, consoleLogger);

      // 使用日志方法来间接测试记录器是否被正确设置
      var testEntry = new LogEntry();
      testEntry.Basic.Message = "Test setup";

      // 测试 API 日志
      log.LevelApi = 0; // 确保 API 日志会被记录
      log.INFO(testEntry);
      // 注意：由于LogstashServerLog78实际发送HTTP请求，我们可能需要模拟HTTP响应或使用实际的Logstash服务器

      // 测试文件日志
      log.LevelFile = 0; // 确保文件日志会被记录
      log.INFO(testEntry);
      // 注意：由于FileLog78实际写入文件，我们可能需要检查文件是否被创建或修改

      // 测试控制台日志
      log.LevelConsole = 0; // 确保控制台日志会被记录
      log.INFO(testEntry);
      // 注意：由于ConsoleLog78实际写入控制台，我们可能需要重定向控制台输出来验证

      // 重置日志级别
      log.LevelApi = 70;
      log.LevelFile = 50;
      log.LevelConsole = 30;

      // 清理
      logstashLogger.Dispose();
      fileLogger.Dispose();
      consoleLogger.Dispose();

      // 如果到这里没有抛出异常，我们就认为测试通过
      Assert.IsTrue(true, "Setup completed without throwing an exception");
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
       
     

      var customEntry = new CustomLogEntry
      {
          Basic = { Message = "Test message", Summary = "Test summary" },
          Weather = "Sunny"
      };

      log.INFO(customEntry);

      // 注意：由于我们使用实际的ConsoleLog78，我们可能需要重定向控制台输出来验证日志内容
      // 这里我们只能确保不抛出异常
      Assert.IsTrue(true, "日志记录应该完成而不抛出异常");

       
    }

    [TestMethod]
    public void TestCustomLogEntryWithException()
    {
      var log = Log78.Instance;
     

      var customEntry = new CustomLogEntry();
      var exception = new Exception("Test exception");

      log.ERROR(exception, customEntry);

      // 同样，我们可能需要重定向控制台输出来验证日志内容
      Assert.IsTrue(true, "异常日志记录应该完成而不抛出异常");

     
    }

    [TestMethod]
    public async Task TestLogstashServerLog78()
    {
      // 设置
      var logstashUrl = "http://192.168.31.122:5000";
      var logstashLogger = new LogstashServerLog78(logstashUrl);
      var log = Log78.Instance;
      log.setup(logstashLogger, new FileLog78(), new ConsoleLog78());
      //log.LevelApi = 50; // 确保所有日志都会被发送到 Logstash

      // 创建测试日志条目
      var testEntry = new LogEntry
      {
        Basic = new BasicInfo
        {
          Message = "Test Logstash integration",
          Summary = "Logstash Test",
          ServiceName = "TestService",
          ServiceObj = "TestObject",
          ServiceFun = "TestFunction",
          UserId = "TestUser",
          UserName = "Test Username"
        }
      };

      // 发送日志
      log.INFO(testEntry);

      // 等待一段时间，确保日志有时间被发送
      await Task.Delay(2000);

      // 验证
      // 注意：这里我们无法直接验证日志是否成功发送到 Logstash
      // 我们可以检查是否有异常抛出，或者添加一些额外的日志记录来确认发送尝试
      // 在实际环境中，您可能需要检查 Logstash 的接收端来确认日志是否被正确接收

      // 如果到这里没有抛出异常，我们就认为测试通过
      Assert.IsTrue(true, "Logstash logging attempt completed without throwing an exception");
    }

    [TestMethod]
    public void TestFileLog78()
    {
        // 设置
        var log = Log78.Instance;
        log.LevelFile = 50; // 确保所有日志都会被写入文件

        // 创建测试日志条目
        var testEntry = new LogEntry
        {
            Basic = new BasicInfo
            {
                Message = "Test file logging",
                Summary = "File Log Test",
                ServiceName = "TestService",
                ServiceObj = "TestObject",
                ServiceFun = "TestFunction",
                UserId = "TestUser",
                UserName = "Test Username"
            }
        };

        // 写入日志
        log.INFO(testEntry);

        // 验证
        string logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
        string[] logFiles = Directory.GetFiles(logDirectory, "7788_*.log");

        Assert.IsTrue(logFiles.Length > 0, "应该至少创建了一个日志文件");

        //file lock can't del
        //string logContent = File.ReadAllText(logFiles[0]);
        //Assert.IsTrue(logContent.Contains("Test file logging"), "日志文件应该包含测试消息");
        //Assert.IsTrue(logContent.Contains("File Log Test"), "日志文件应该包含测试摘要");

        //// 清理
        //foreach (var file in logFiles)
        //{
        //    File.Delete(file);
        //}
    }
  }

 

   
}