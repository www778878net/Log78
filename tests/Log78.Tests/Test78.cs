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
        private static Log78 logger;

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
            logger.SetEnvironment(Environment.Development);
            // ... (其余测试代码)
            await Task.CompletedTask; // 添加这行以使方法成为真正的异步方法
        }

        [TestMethod]
        public async Task TestLogLevels()
        {
            await logger.Debug("Debug message");
            // ... (其余测试代码)
        }

        [TestMethod]
        public async Task TestCustomLogEntry()
        {
            var customEntry = new LogEntry
            {
                // ... (设置 LogEntry 属性)
            };

            await logger.InfoEntry(customEntry);
            // ... (其余测试代码)
        }

        [TestMethod]
        public async Task TestLogstashServerLog78()
        {
            var serverLog = new LogstashServerLog78("http://example.com", 50);
            // ... (其余测试代码)
            await Task.CompletedTask; // 添加这行以使方法成为真正的异步方法
        }

        // ... (其他测试方法)
    }
}