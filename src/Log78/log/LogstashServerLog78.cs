using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace www778878net.log
{
    /// <summary>
    /// Logstash server logger
    /// *Note: If errorLevel is greater than or equal to Log78's LevelApi, it may cause an infinite loop when sending logs to Logstash fails
    /// </summary>
    public class LogstashServerLog78 : IServerLog78, IDisposable
    {
        public string ServerUrl { get; set; }
        private readonly HttpClient _httpClient;
        private readonly Log78 _logger;
      
        /// <summary>
        /// 
        /// </summary>
        /// <param name="serverUrl"></param>
        /// <param name="errorLevel">Must be less than Log78's LevelApi to avoid potential infinite loops</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when errorLevel is greater than or equal to Log78's LevelApi</exception>
        public LogstashServerLog78(string serverUrl,int LevelFile=50)
        {
            ServerUrl = serverUrl;
           
            _httpClient = new HttpClient();
            _logger =  new Log78();
            _logger.LevelApi=99999;//直接就没设置 我们就是
            _logger.LevelConsole=LevelFile;//必然是出错了
            _logger.LevelFile=LevelFile;//必然是出错了
        }

        private void ValidateErrorLevel(int errorLevel)
        {
            if (errorLevel >= Log78.Instance.LevelApi)
            {
                throw new ArgumentOutOfRangeException(nameof(errorLevel), 
                    $"Error level must be less than Log78's LevelApi ({Log78.Instance.LevelApi}). Current value ({errorLevel}) may cause an infinite loop when sending logs to Logstash fails.");
            }
        }

        public async void LogToServer(LogEntry logEntry)
        {
            try
            {
                string jsonContent = logEntry.ToJson();
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(ServerUrl, content);

                if (response.IsSuccessStatusCode){
                  _logger.DEBUG("ok");
                  return;
                }
                
                var errorEntry = new LogEntry
                {
                    Basic = new BasicInfo
                    {
                        Message = $"Failed to send log to Logstash. Status code: {response.StatusCode}",
                        Summary = "Logstash Error",
                        LogLevel = "ERROR",                            
                    }
                };
                _logger.ERROR(errorEntry, 50);  // 使用最低级别，避免再次发送到服务器
                
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
                _logger.ERROR(errorEntry, 50);  // 使用最低级别，避免再次发送到服务器
            }
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}