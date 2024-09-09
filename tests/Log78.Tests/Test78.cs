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
using Newtonsoft.Json.Linq;
using System.Linq;
using Newtonsoft.Json;

namespace Test78
{
    [TestClass]
    public class Log78Tests
    {
        private static Log78? logger;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            logger = Log78.Instance;
            logger.SetupDetailFile();
            logger.ClearDetailLog();
        }

        [TestMethod]
        public async Task TestEnvironmentSettings()
        {
            logger?.SetEnvironment(Log78.Environment.Development);
            // ... (������Դ���)
            await Task.CompletedTask;
        }

        [TestMethod]
        public async Task TestLogLevels()
        {
            var logEntry = new LogEntry
            {
                Basic = new BasicInfo
                {
                    Message = "Debug message",
                    LogLevel = "DEBUG",
                    LogLevelNumber = 20
                }
            };
            await logger!.DEBUG(logEntry);
            // ... (������Դ���)
        }

        [TestMethod]
        public async Task TestCustomLogEntry()
        {
            var customEntry = new LogEntry
            {
                Basic = new BasicInfo
                {
                    Message = "Custom log entry",
                    LogLevel = "INFO",
                    LogLevelNumber = 30
                }
                // ... (�������� LogEntry ����)
            };

            await logger!.INFO(customEntry);
            // ... (������Դ���)
        }

        [TestMethod]
        public async Task TestLogstashServerLog78()
        {
            var serverLog = new LogstashServerLog78("http://example.com", 50);
            // ... (������Դ���)
            await Task.CompletedTask;
        }

        [TestMethod]
        public async Task TestClone()
        {
            var originalLogger = Log78.Instance;
            originalLogger.SetEnvironment(Log78.Environment.Development);

            var clonedLogger = originalLogger.Clone();

            // ��֤����logger����������ͬ��
            Assert.AreEqual(originalLogger.CurrentEnvironment, clonedLogger.CurrentEnvironment);
            Assert.AreEqual(originalLogger.GetCurrentLevels(), clonedLogger.GetCurrentLevels());

            // ��֤����ԭʼlogger�����ò���Ӱ���¡��logger
            originalLogger.SetEnvironment(Log78.Environment.Production);
            Assert.AreNotEqual(originalLogger.CurrentEnvironment, clonedLogger.CurrentEnvironment);

            await Task.CompletedTask;
        }

        [TestMethod]
        public async Task TestDetailLogAlwaysWritten()
        {
            var logger = Log78.Instance;
            logger.SetEnvironment(Log78.Environment.Production);
            logger.SetupDetailFile();

            var logEntry = new LogEntry
            {
                Basic = new BasicInfo
                {
                    Message = "Test detail log",
                    LogLevel = "INFO",
                    LogLevelNumber = 30
                }
            };

            await logger.INFO(logEntry);

            // ��֤��ϸ��־�ļ����ڲ�������־��Ŀ
            string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs", "detail.log");
            Assert.IsTrue(File.Exists(logPath), "Detail log file should exist");
            string logContent = File.ReadAllText(logPath);
            Console.WriteLine($"Log content: {logContent}"); // ��������������־����
            Assert.IsTrue(logContent.Contains("Test detail log"), "Detail log should contain the log message");
        }

        [TestMethod]
        public async Task TestLogMethodOverloads()
        {
            var logger = Log78.Instance;
            logger.SetEnvironment(Log78.Environment.Development);
            logger.SetupDetailFile();

            await logger.DETAIL("Detail summary", new { DetailInfo = "Some detail info" });
            await logger.DEBUG("Debug summary", new { DebugInfo = "Some debug info" });
            await logger.INFO("Info summary", new { InfoData = "Some info data" });
            await logger.WARN("Warn summary", new { WarnCode = 123 });
            await logger.ERROR("Error summary", new { ErrorCode = 500 });

            try
            {
                throw new Exception("Test exception");
            }
            catch (Exception ex)
            {
                await logger.ERROR(ex, "Custom error summary");
            }

            // ��֤��ϸ��־�ļ����ڲ�����������־��Ϣ
            string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs", "detail.log");
            Assert.IsTrue(File.Exists(logPath), "Detail log file should exist");
            string logContent = File.ReadAllText(logPath);
            Console.WriteLine($"Log content: {logContent}");
            Assert.IsTrue(logContent.Contains("Detail summary"), "Log should contain Detail summary");
            Assert.IsTrue(logContent.Contains("Some detail info"), "Log should contain detail info");
            Assert.IsTrue(logContent.Contains("Debug summary"), "Log should contain Debug summary");
            Assert.IsTrue(logContent.Contains("Some debug info"), "Log should contain debug info");
            Assert.IsTrue(logContent.Contains("Info summary"), "Log should contain Info summary");
            Assert.IsTrue(logContent.Contains("Some info data"), "Log should contain info data");
            Assert.IsTrue(logContent.Contains("Warn summary"), "Log should contain Warn summary");
            Assert.IsTrue(logContent.Contains("123"), "Log should contain warn code");
            Assert.IsTrue(logContent.Contains("Error summary"), "Log should contain Error summary");
            Assert.IsTrue(logContent.Contains("500"), "Log should contain error code");
            Assert.IsTrue(logContent.Contains("Custom error summary"), "Log should contain custom error summary");
            Assert.IsTrue(logContent.Contains("Test exception"), "Log should contain exception message");
        }

        [TestMethod]
        public async Task TestMessageSerialization()
        {
            var logger = Log78.Instance;
            logger.SetEnvironment(Log78.Environment.Development);
            logger.SetupDetailFile();

            // �����ַ�����Ϣ
            await logger.INFO("String message test", "This is a string message");

            // ���Զ�����Ϣ
            await logger.INFO("Object message test", new { Key = "Value", Number = 123 });

            // ��֤��ϸ��־�ļ����ڲ�����������־��Ϣ
            string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs", "detail.log");
            Assert.IsTrue(File.Exists(logPath), "Detail log file should exist");
            string logContent = File.ReadAllText(logPath);
            Console.WriteLine($"Log content: {logContent}");
            
            // ������־����
            var logEntries = logContent.Split(new[] { "<AI_FOCUS_LOG>", "</AI_FOCUS_LOG>" }, StringSplitOptions.RemoveEmptyEntries)
                                       .Where(json => !string.IsNullOrWhiteSpace(json))
                                       .Select(json => JObject.Parse(json))
                                       .ToList();

            Assert.IsTrue(logEntries.Count > 0, "At least one log entry should be parsed");

            // ��֤�ַ�����Ϣ
            var stringMessageEntry = logEntries.FirstOrDefault(e => e["summary"]?.ToString() == "String message test");
            Assert.IsNotNull(stringMessageEntry, "String message entry should exist");
            Assert.AreEqual("This is a string message", stringMessageEntry["message"]?.ToString(), "String message should be correct");

            // ��֤������Ϣ
            var objectMessageEntry = logEntries.FirstOrDefault(e => e["summary"]?.ToString() == "Object message test");
            Assert.IsNotNull(objectMessageEntry, "Object message entry should exist");
            Assert.IsTrue(objectMessageEntry["message"] is JObject, "Object message should be a JSON object");
            var messageObject = objectMessageEntry["message"] as JObject;
            Assert.IsNotNull(messageObject, "Message object should not be null");
            Assert.AreEqual("Value", messageObject["Key"]?.ToString(), "Object message should contain correct key");
            Assert.AreEqual("123", messageObject["Number"]?.ToString(), "Object message should contain correct number");
        }

        [TestMethod]
        public async Task TestDynamicLogLevelAdjustment()
        {
            var logger = Log78.Instance;
            logger.SetEnvironment(Log78.Environment.Development);
            logger.SetupDetailFile();

            // �����Զ�����־����
            logger.SetupLevel(40, 50, 60);

            // ���Բ�ͬ�������־
            await logger.DETAIL("Detail message", new { Level = "DETAIL" });
            await logger.DEBUG("Debug message", new { Level = "DEBUG" });
            await logger.INFO("Info message", new { Level = "INFO" });
            await logger.WARN("Warn message", new { Level = "WARN" });
            await logger.ERROR("Error message", new { Level = "ERROR" });

            // ��֤��ϸ��־�ļ����ڲ�����������־��Ϣ
            string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs", "detail.log");
            Assert.IsTrue(File.Exists(logPath), "Detail log file should exist");
            string logContent = File.ReadAllText(logPath);
            Console.WriteLine($"Log content: {logContent}");

            // ��֤��־���������Ƿ���Ч
            Assert.IsTrue(logContent.Contains("Detail message"), "Log should contain Detail message");
            Assert.IsTrue(logContent.Contains("Debug message"), "Log should contain Debug message");
            Assert.IsTrue(logContent.Contains("Info message"), "Log should contain Info message");
            Assert.IsTrue(logContent.Contains("Warn message"), "Log should contain Warn message");
            Assert.IsTrue(logContent.Contains("Error message"), "Log should contain Error message");

            // ��֤��־����
            var (fileLevel, consoleLevel, apiLevel) = logger.GetCurrentLevels();
            Assert.AreEqual(40, fileLevel, "File log level should be 40");
            Assert.AreEqual(50, consoleLevel, "Console log level should be 50");
            Assert.AreEqual(60, apiLevel, "API log level should be 60");
        }

        [TestMethod]
        public async Task TestDebugEntry()
        {
            var logger = Log78.Instance;
            logger.SetEnvironment(Log78.Environment.Development);
            logger.SetupDetailFile();

            // ���õ�������
            logger.DebugEntry = new LogEntry
            {
                Basic = new BasicInfo { UserName = "guest" }
            };

            // ��¼һ�������������������־
            await logger.INFO("Normal log", new { UserName = "admin" });

            // ��¼һ�����������������־
            await logger.INFO("Debug log", new { UserName = "guest" });

            // ��֤��ϸ��־�ļ����ڲ�����������־��Ϣ
            string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs", "detail.log");
            Assert.IsTrue(File.Exists(logPath), "Detail log file should exist");
            string logContent = File.ReadAllText(logPath);
            Console.WriteLine($"Log content: {logContent}");

            // ������־����
            var logEntries = logContent.Split(new[] { "<AI_FOCUS_LOG>", "</AI_FOCUS_LOG>" }, StringSplitOptions.RemoveEmptyEntries)
                                       .Where(json => !string.IsNullOrWhiteSpace(json))
                                       .Select(json => JObject.Parse(json))
                                       .ToList();

            Assert.IsTrue(logEntries.Count > 0, "At least one log entry should be parsed");

            // ��֤������־
            var debugLogEntry = logEntries.FirstOrDefault(e => e["summary"]?.ToString() == "Debug log");
            Assert.IsNotNull(debugLogEntry, "Debug log entry should exist");
            Assert.AreEqual("guest", debugLogEntry["message"]?["UserName"]?.ToString(), "Debug log should contain correct username");

            // ��֤��ͨ��־�Ƿ񱻼�¼��ȡ���ڵ�ǰ����־�������ã�
            var normalLogEntry = logEntries.FirstOrDefault(e => e["summary"]?.ToString() == "Normal log");
            if (logger.GetCurrentLevels().FileLevel <= 30) // INFO level
            {
                Assert.IsNotNull(normalLogEntry, "Normal log entry should exist if file log level is INFO or lower");
            }
            else
            {
                Assert.IsNull(normalLogEntry, "Normal log entry should not exist if file log level is higher than INFO");
            }
        }

        [TestMethod]
        public async Task TestDebugKind()
        {
            var logger = Log78.Instance;
            logger.SetEnvironment(Log78.Environment.Development);
            logger.SetupDetailFile();

            // ��ӵ��Լ�
            logger.AddDebugKey("testuser");

            // ��¼һ�������������������־
            await logger.INFO("Normal log", new { UserName = "admin" });

            // ��¼һ�����������������־
            await logger.INFO("Debug log", new { UserName = "testuser" });

            // ��֤��ϸ��־�ļ����ڲ�����������־��Ϣ
            string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs", "detail.log");
            Assert.IsTrue(File.Exists(logPath), "Detail log file should exist");
            string logContent = File.ReadAllText(logPath);
            Console.WriteLine($"Log content: {logContent}");

            // ������־����
            var logEntries = logContent.Split(new[] { "<AI_FOCUS_LOG>", "</AI_FOCUS_LOG>" }, StringSplitOptions.RemoveEmptyEntries)
                                       .Where(json => !string.IsNullOrWhiteSpace(json))
                                       .Select(json => JObject.Parse(json))
                                       .ToList();

            Assert.IsTrue(logEntries.Count > 0, "At least one log entry should be parsed");

            // ��֤������־
            var debugLogEntry = logEntries.FirstOrDefault(e => e["summary"]?.ToString() == "Debug log");
            Assert.IsNotNull(debugLogEntry, "Debug log entry should exist");
            Assert.AreEqual("testuser", debugLogEntry["message"]?["UserName"]?.ToString(), "Debug log should contain correct username");

            // ��֤��ͨ��־�Ƿ񱻼�¼��ȡ���ڵ�ǰ����־�������ã�
            var normalLogEntry = logEntries.FirstOrDefault(e => e["summary"]?.ToString() == "Normal log");
            if (logger.GetCurrentLevels().FileLevel <= 30) // INFO level
            {
                Assert.IsNotNull(normalLogEntry, "Normal log entry should exist if file log level is INFO or lower");
            }
            else
            {
                Assert.IsNull(normalLogEntry, "Normal log entry should not exist if file log level is higher than INFO");
            }
        }

        // ... (�������Է���)
    }
}