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
            // ... (其余测试代码)
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
            // ... (其余测试代码)
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
                // ... (设置其他 LogEntry 属性)
            };

            await logger!.INFO(customEntry);
            // ... (其余测试代码)
        }

        [TestMethod]
        public async Task TestLogstashServerLog78()
        {
            var serverLog = new LogstashServerLog78("http://example.com", 50);
            // ... (其余测试代码)
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

            // 验证两个logger的设置是相同的
            Assert.AreEqual(originalLogger.CurrentEnvironment, clonedLogger.CurrentEnvironment);
            Assert.AreEqual(originalLogger.GetCurrentLevels(), clonedLogger.GetCurrentLevels());
            
            // 验证 DebugEntry 是共享的
            Assert.AreSame(originalLogger.DebugEntry, clonedLogger.DebugEntry);

            // 验证更改原始logger的DebugEntry也会影响克隆的logger
            originalLogger.DebugEntry = new LogEntry { Basic = new BasicInfo { Message = "New Debug Entry" } };
            Assert.AreSame(originalLogger.DebugEntry, clonedLogger.DebugEntry);

            await Task.CompletedTask;
        }

        // ... (其他测试方法)
    }
}