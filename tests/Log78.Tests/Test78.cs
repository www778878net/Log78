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
        [TestInitialize]
        public void TestInitialize()
        {
            // ���� Log78 ����ʵ��
            typeof(Log78).GetField("instance", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static).SetValue(null, null);
        }

        [TestMethod]
        public void TestSingleton()
        {
            var instance1 = Log78.Instance;
            var instance2 = Log78.Instance;
            Assert.AreSame(instance1, instance2, "����ģʽӦ�÷�����ͬ��ʵ��");
        }

        [TestMethod]
        public async Task TestSetup()
        {
            var log = Log78.Instance;
            var fileLogger = new FileLog78("7788_.log", "testlogs");
            var consoleLogger = new ConsoleLog78();

            log.setup(null, fileLogger, consoleLogger);

            var testEntry = new LogEntry { Basic = new BasicInfo { Message = "Test setup" } };

            await log.INFO(testEntry);

            string logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testlogs");
            string[] logFiles = Directory.GetFiles(logDirectory, "7788_*.log");

            Assert.IsTrue(logFiles.Length > 0, "Ӧ�����ٴ�����һ����־�ļ�");

            fileLogger.Dispose();
            consoleLogger.Dispose();
        }

        [TestMethod]
        public async Task TestEnvironmentSettings()
        {
            Environment.SetEnvironmentVariable("LOG78_ENVIRONMENT", "Development");
            var log = new Log78();
            
            var (fileLevel, consoleLevel, apiLevel) = log.GetCurrentLevels();
            Assert.AreEqual(20, consoleLevel);
            Assert.AreEqual(20, fileLevel);
            Assert.AreEqual(50, apiLevel);

            log.SetEnvironment(Log78.Environment.Production);
            (fileLevel, consoleLevel, apiLevel) = log.GetCurrentLevels();
            Assert.AreEqual(60, consoleLevel);
            Assert.AreEqual(30, fileLevel);
            Assert.AreEqual(50, apiLevel);

            log.SetEnvironment(Log78.Environment.Testing);
            (fileLevel, consoleLevel, apiLevel) = log.GetCurrentLevels();
            Assert.AreEqual(60, consoleLevel);
            Assert.AreEqual(20, fileLevel);
            Assert.AreEqual(50, apiLevel);

            // ���Կ��������ĵ����ļ���־
            log.SetEnvironment(Log78.Environment.Development);
            string debugMessage = "Debug message for file";
            await log.DEBUG(new LogEntry { Basic = new BasicInfo { Message = debugMessage } });
            
            string debugLogPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs", "detail.log");
            Assert.IsTrue(File.Exists(debugLogPath), $"Debug log file should exist at {debugLogPath}");
            if (File.Exists(debugLogPath))
            {
                string debugLogContent = File.ReadAllText(debugLogPath);
                Assert.IsTrue(debugLogContent.Contains(debugMessage), "Debug log should contain the debug message");
            }
        }

        [TestMethod]
        public async Task TestLogLevels()
        {
            var log = Log78.Instance;
            log.SetupDetailFile();

            var testEntry = new LogEntry { Basic = new BasicInfo { Message = "Test log levels" } };

            await log.DETAIL(testEntry, 10);
            await log.DEBUG(testEntry, 20);
            await log.INFO(testEntry, 30);
            await log.WARN(testEntry, 50);
            await log.ERROR(testEntry, 60);

            string detailLogPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs", "detail.log");
            Assert.IsTrue(File.Exists(detailLogPath), "Detail log file should exist");

            if (File.Exists(detailLogPath))
            {
                string logContent = File.ReadAllText(detailLogPath);
                Assert.IsTrue(logContent.Contains("DETAIL"), "Log should contain DETAIL level");
                Assert.IsTrue(logContent.Contains("DEBUG"), "Log should contain DEBUG level");
                Assert.IsTrue(logContent.Contains("INFO"), "Log should contain INFO level");
                Assert.IsTrue(logContent.Contains("WARN"), "Log should contain WARN level");
                Assert.IsTrue(logContent.Contains("ERROR"), "Log should contain ERROR level");
            }
        }

        [TestMethod]
        public async Task TestCustomLogEntry()
        {
            var log = Log78.Instance;
            log.SetupDetailFile();

            var customEntry = new CustomLogEntry
            {
                Basic = { Message = "Test message", Summary = "Test summary" },
                Weather = "Sunny"
            };

            await log.INFO(customEntry);

            string detailLogPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs", "detail.log");
            Assert.IsTrue(File.Exists(detailLogPath), "Detail log file should exist");

            if (File.Exists(detailLogPath))
            {
                string logContent = File.ReadAllText(detailLogPath);
                Assert.IsTrue(logContent.Contains("Test message"), "Log should contain the test message");
                Assert.IsTrue(logContent.Contains("Sunny"), "Log should contain the custom weather field");
            }
        }

        [TestMethod]
        public async Task TestLogstashServerLog78()
        {
            var logstashUrl = "http://localhost:5000"; // ʹ�ñ��ز���URL
            var logstashLogger = new LogstashServerLog78(logstashUrl);
            var log = Log78.Instance;
            log.setup(logstashLogger, new FileLog78(), new ConsoleLog78());

            var testEntry = new LogEntry
            {
                Basic = new BasicInfo
                {
                    Message = "Test Logstash integration",
                    Summary = "Logstash Test",
                    ServiceName = "TestService"
                }
            };

            await log.INFO(testEntry);

            // ע�⣺��������ֻ�ǲ����Ƿ�û���׳��쳣
            // ʵ����Ŀ�У��������Ҫһ��ģ���Logstash����������֤��־�Ƿ���ȷ����
            Assert.IsTrue(true, "Logstash logging attempt completed without throwing an exception");
        }
    }
}