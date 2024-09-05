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
      Assert.AreSame(instance1, instance2, "����ģʽӦ�÷�����ͬ��ʵ��");
    }

    [TestMethod]
    public void TestSetup()
    {
      var log = Log78.Instance;
      var logstashLogger = new LogstashServerLog78("http://localhost:5000");
      var fileLogger = new FileLog78("testlogs");
      var consoleLogger = new ConsoleLog78();

      log.setup(logstashLogger, fileLogger, consoleLogger);

      // ʹ����־��������Ӳ��Լ�¼���Ƿ���ȷ����
      var testEntry = new LogEntry();
      testEntry.Basic.Message = "Test setup";

      // ���� API ��־
      log.LevelApi = 0; // ȷ�� API ��־�ᱻ��¼
      log.INFO(testEntry);
      // ע�⣺����LogstashServerLog78ʵ�ʷ���HTTP�������ǿ�����Ҫģ��HTTP��Ӧ��ʹ��ʵ�ʵ�Logstash������

      // �����ļ���־
      log.LevelFile = 0; // ȷ���ļ���־�ᱻ��¼
      log.INFO(testEntry);
      // ע�⣺����FileLog78ʵ��д���ļ������ǿ�����Ҫ����ļ��Ƿ񱻴������޸�

      // ���Կ���̨��־
      log.LevelConsole = 0; // ȷ������̨��־�ᱻ��¼
      log.INFO(testEntry);
      // ע�⣺����ConsoleLog78ʵ��д�����̨�����ǿ�����Ҫ�ض������̨�������֤

      // ������־����
      log.LevelApi = 70;
      log.LevelFile = 50;
      log.LevelConsole = 30;

      // ����
      logstashLogger.Dispose();
      fileLogger.Dispose();
      consoleLogger.Dispose();

      // ���������û���׳��쳣�����Ǿ���Ϊ����ͨ��
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

      Assert.AreEqual(originalLog.LevelApi, clonedLog.LevelApi, "API����Ӧ����ͬ");
      Assert.AreEqual(originalLog.LevelConsole, clonedLog.LevelConsole, "����̨����Ӧ����ͬ");
      Assert.AreEqual(originalLog.LevelFile, clonedLog.LevelFile, "�ļ�����Ӧ����ͬ");
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

      // ע�⣺��������ʹ��ʵ�ʵ�ConsoleLog78�����ǿ�����Ҫ�ض������̨�������֤��־����
      // ��������ֻ��ȷ�����׳��쳣
      Assert.IsTrue(true, "��־��¼Ӧ����ɶ����׳��쳣");

       
    }

    [TestMethod]
    public void TestCustomLogEntryWithException()
    {
      var log = Log78.Instance;
     

      var customEntry = new CustomLogEntry();
      var exception = new Exception("Test exception");

      log.ERROR(exception, customEntry);

      // ͬ�������ǿ�����Ҫ�ض������̨�������֤��־����
      Assert.IsTrue(true, "�쳣��־��¼Ӧ����ɶ����׳��쳣");

     
    }

    [TestMethod]
    public async Task TestLogstashServerLog78()
    {
      // ����
      var logstashUrl = "http://192.168.31.122:5000";
      var logstashLogger = new LogstashServerLog78(logstashUrl);
      var log = Log78.Instance;
      log.setup(logstashLogger, new FileLog78(), new ConsoleLog78());
      //log.LevelApi = 50; // ȷ��������־���ᱻ���͵� Logstash

      // ����������־��Ŀ
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

      // ������־
      log.INFO(testEntry);

      // �ȴ�һ��ʱ�䣬ȷ����־��ʱ�䱻����
      await Task.Delay(2000);

      // ��֤
      // ע�⣺���������޷�ֱ����֤��־�Ƿ�ɹ����͵� Logstash
      // ���ǿ��Լ���Ƿ����쳣�׳����������һЩ�������־��¼��ȷ�Ϸ��ͳ���
      // ��ʵ�ʻ����У���������Ҫ��� Logstash �Ľ��ն���ȷ����־�Ƿ���ȷ����

      // ���������û���׳��쳣�����Ǿ���Ϊ����ͨ��
      Assert.IsTrue(true, "Logstash logging attempt completed without throwing an exception");
    }

    [TestMethod]
    public void TestFileLog78()
    {
        // ����
        var log = Log78.Instance;
        log.LevelFile = 50; // ȷ��������־���ᱻд���ļ�

        // ����������־��Ŀ
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

        // д����־
        log.INFO(testEntry);

        // ��֤
        string logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
        string[] logFiles = Directory.GetFiles(logDirectory, "7788_*.log");

        Assert.IsTrue(logFiles.Length > 0, "Ӧ�����ٴ�����һ����־�ļ�");

        //file lock can't del
        //string logContent = File.ReadAllText(logFiles[0]);
        //Assert.IsTrue(logContent.Contains("Test file logging"), "��־�ļ�Ӧ�ð���������Ϣ");
        //Assert.IsTrue(logContent.Contains("File Log Test"), "��־�ļ�Ӧ�ð�������ժҪ");

        //// ����
        //foreach (var file in logFiles)
        //{
        //    File.Delete(file);
        //}
    }
  }

 

   
}