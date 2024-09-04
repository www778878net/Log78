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
      Assert.AreSame(instance1, instance2, "����ģʽӦ�÷�����ͬ��ʵ��");
    }

    [TestMethod]
    public void TestSetup()
    {
      var log = Log78.Instance;
      var mockServerLogger = new MockServerLogger();
      var mockFileLogger = new MockFileLogger();
      var mockConsoleLogger = new MockConsoleLogger();

      log.setup(mockServerLogger, mockFileLogger, mockConsoleLogger);

      // ʹ����־��������Ӳ��Լ�¼���Ƿ���ȷ����
      var testEntry = new LogEntry();
      testEntry.Basic.Message = "Test setup";

      // ���� API ��־
      log.LevelApi = 0; // ȷ�� API ��־�ᱻ��¼
      log.INFO(testEntry);
      Assert.IsTrue(mockServerLogger.WasLogCalled, "��������־��¼��Ӧ�ñ�����");

      // �����ļ���־
      log.LevelFile = 0; // ȷ���ļ���־�ᱻ��¼
      log.INFO(testEntry);
      Assert.IsTrue(mockFileLogger.WasLogCalled, "�ļ���־��¼��Ӧ�ñ�����");

      // ���Կ���̨��־
      log.LevelConsole = 0; // ȷ������̨��־�ᱻ��¼
      log.INFO(testEntry);
      Assert.IsTrue(mockConsoleLogger.WasLogCalled, "����̨��־��¼��Ӧ�ñ�����");

      // ������־����
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

      Assert.AreEqual(originalLog.LevelApi, clonedLog.LevelApi, "API����Ӧ����ͬ");
      Assert.AreEqual(originalLog.LevelConsole, clonedLog.LevelConsole, "����̨����Ӧ����ͬ");
      Assert.AreEqual(originalLog.LevelFile, clonedLog.LevelFile, "�ļ�����Ӧ����ͬ");
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

      Assert.IsTrue(mockConsoleLogger.WasLogCalled, "����̨��־Ӧ������");
      Assert.IsTrue(mockConsoleLogger.LastLoggedMessage.Contains("Test message"), "��ϢӦ�ñ���¼");
      Assert.IsTrue(mockConsoleLogger.LastLoggedMessage.Contains("Test summary"), "ժҪӦ�ñ���¼");
      Assert.IsTrue(mockConsoleLogger.LastLoggedMessage.Contains("Sunny"), "����Ӧ�ñ���¼");
      Assert.IsTrue(mockConsoleLogger.LastLoggedMessage.Contains(Environment.MachineName), "������Ӧ�ñ���¼");
      Assert.IsTrue(mockConsoleLogger.LastLoggedMessage.Contains(Environment.UserName), "�û���Ӧ�ñ���¼");
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

      Assert.IsTrue(mockConsoleLogger.WasLogCalled, "����̨��־Ӧ������");
      Assert.IsTrue(mockConsoleLogger.LastLoggedMessage.Contains("Test exception"), "�쳣��ϢӦ�ñ���¼");
      Assert.IsTrue(mockConsoleLogger.LastLoggedMessage.Contains(Environment.MachineName), "������Ӧ�ñ���¼");
      Assert.IsTrue(mockConsoleLogger.LastLoggedMessage.Contains(Environment.UserName), "�û���Ӧ�ñ���¼");
    }
  }

  // ģ�����ʵ��
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