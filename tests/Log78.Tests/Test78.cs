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
            var originalDebugEntry = new LogEntry { Basic = new BasicInfo { Message = "Original Debug Entry" } };
            originalLogger.DebugEntry = originalDebugEntry;

            var clonedLogger = originalLogger.Clone();

            // ��֤����logger����������ͬ��
            Assert.AreEqual(originalLogger.CurrentEnvironment, clonedLogger.CurrentEnvironment);
            Assert.AreEqual(originalLogger.GetCurrentLevels(), clonedLogger.GetCurrentLevels());
            
            // ��֤ DebugEntry �ǹ����
            Assert.AreSame(originalLogger.DebugEntry, clonedLogger.DebugEntry);

            // ��֤����ԭʼlogger��DebugEntryҲ��Ӱ���¡��logger
            originalLogger.DebugEntry = new LogEntry { Basic = new BasicInfo { Message = "New Debug Entry" } };
            Assert.AreSame(originalLogger.DebugEntry, clonedLogger.DebugEntry);

            await Task.CompletedTask;
        }

        // ... (�������Է���)
    }
}