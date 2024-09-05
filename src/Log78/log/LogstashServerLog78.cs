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
        public LogstashServerLog78(string serverUrl, int levelFile = 50)
        {
            ServerUrl = serverUrl;
           
            _httpClient = new HttpClient();
            _logger = new Log78();
            _logger.LevelApi = 99999; // 直接就没设置 我们就是
            _logger.LevelConsole = levelFile; // 必然是出错了
            _logger.LevelFile = levelFile; // 必然是出错了
        }

        public async Task LogToServer(LogEntry logEntry)
        {
            try
            {
                string jsonContent = logEntry.ToJson();
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(ServerUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    await _logger.DEBUG("Logstash log sent successfully", "Logstash Success");
                }
                else
                {
                    var errorMessage = $"Failed to send log to Logstash. Status code: {response.StatusCode}";
                    await _logger.ERROR(errorMessage, "Logstash Error");
                    throw new HttpRequestException(errorMessage);
                }
            }
            catch (Exception ex)
            {
                var errorMessage = $"Error sending log to Logstash: {ex.Message}";
                await _logger.ERROR(errorMessage, "Logstash Exception");
                throw;
            }
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}