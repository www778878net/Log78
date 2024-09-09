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
            
            Assert.IsTrue(logContent.Contains("\"message\":\"This is a string message\""), "Log should contain the string message as-is");
            Assert.IsTrue(logContent.Contains("\"message\":{\"key\":\"Value\",\"number\":123}"), "Log should contain the object message as JSON");
        }

        // ... (�������Է���)
    }
}