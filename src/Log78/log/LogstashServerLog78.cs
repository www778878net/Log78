using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace www778878net.log
{
    /// <summary>
    /// Logstash 日志服务器 
    /// *注意Log78的LevelApi 如果设置为50以下，发送到Logstash的日志失败会死循环
    /// </summary>
    public class LogstashServerLog78 : IServerLog78, IDisposable
    {
        public string ServerUrl { get; set; }
        private readonly HttpClient _httpClient;
        private readonly Log78 _logger;
        private readonly int _errorLevel;

        public LogstashServerLog78(string serverUrl, int errorLevel = 50)
        {
            ServerUrl = serverUrl;
            _httpClient = new HttpClient();
            _logger = Log78.Instance;
            _errorLevel = errorLevel;
        }

        public async void LogToServer(LogEntry logEntry)
        {
            try
            {
                string jsonContent = logEntry.ToJson();
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(ServerUrl, content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorEntry = new LogEntry
                    {
                        Basic = new BasicInfo
                        {
                            Message = $"Failed to send log to Logstash. Status code: {response.StatusCode}",
                            Summary = "Logstash Error",
                            LogLevel = "ERROR",                            
                        }
                    };
                    _logger.ERROR(errorEntry, _errorLevel);  // 使用最低级别，避免再次发送到服务器
                }
            }
            catch (Exception ex)
            {
                var errorEntry = new LogEntry
                {
                    Basic = new BasicInfo
                    {
                        Message = $"Error sending log to Logstash: {ex.Message}",
                        Summary = "Logstash Exception",
                        LogLevel = "ERROR",
                       
                    }
                };
                _logger.ERROR(errorEntry, _errorLevel);  // 使用最低级别，避免再次发送到服务器
            }
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}