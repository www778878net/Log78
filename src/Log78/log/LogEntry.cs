using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace www778878net.log
{
    /*
    {
  "summary": "用户登录成功",
  "logLevelNumber": 70,
  "timestamp": "2023-05-10T08:30:00Z",
  "logLevel": "INFO",
  "message": "用户 johndoe 成功登录系统",
  "hostName": "web-server-01",
  "serviceName": "AuthService",
  "serviceVersion": "1.2.3",
  "userId": "user123",
  "userName": "johndoe",
  "userEmail": "johndoe@example.com",
  "eventId": "550e8400-e29b-41d4-a716-446655440000",
  "eventKind": "event",
  "eventCategory": "authentication",
  "eventAction": "login",
  "eventOutcome": "success",
  "errorType": null,
  "errorMessage": null,
  "errorStackTrace": null,
  "httpRequestMethod": "POST",
  "httpRequestBodyContent": "{\"username\":\"johndoe\",\"password\":\"*****\"}",
  "httpResponseStatusCode": 200,
  "urlOriginal": "https://api.example.com/login",
  "eventDuration": 150,
  "transactionId": "tx-12345",
  "traceId": "trace-6789",
  "spanId": "span-9876"
}
    */
    public class LogEntry
    {
        public BasicInfo Basic { get; set; } = new BasicInfo();
        public EventInfo Event { get; set; } = new EventInfo();
        public ErrorInfo Error { get; set; } = new ErrorInfo();
        public HttpInfo Http { get; set; } = new HttpInfo();
        public TraceInfo Trace { get; set; } = new TraceInfo();

        [JsonExtensionData]
        public JObject AdditionalProperties { get; set; } = new JObject();

        public string ToJson()
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            var json = JsonConvert.SerializeObject(this, settings);
            var jObject = JObject.Parse(json);

            // 将所有嵌套属性提升到顶层
            FlattenObject(jObject);

            // 移除所有空对象
            RemoveEmptyObjects(jObject);

            return jObject.ToString(Formatting.None);
        }

        private void FlattenObject(JObject jObject)
        {
            var propertiesToAdd = new JObject();
            var propertiesToRemove = new System.Collections.Generic.List<string>();

            foreach (var property in jObject.Properties())
            {
                if (property.Value.Type == JTokenType.Object)
                {
                    var nestedObject = (JObject)property.Value;
                    foreach (var nestedProperty in nestedObject.Properties())
                    {
                        propertiesToAdd.Add(nestedProperty.Name, nestedProperty.Value);
                    }
                    propertiesToRemove.Add(property.Name);
                }
            }

            foreach (var propertyName in propertiesToRemove)
            {
                jObject.Remove(propertyName);
            }

            foreach (var property in propertiesToAdd.Properties())
            {
                jObject.Add(property.Name, property.Value);
            }
        }

        private void RemoveEmptyObjects(JObject jObject)
        {
            var propertiesToRemove = jObject.Properties()
                .Where(p => p.Value.Type == JTokenType.Object && !((JObject)p.Value).Properties().Any())
                .Select(p => p.Name)
                .ToList();

            foreach (var propertyName in propertiesToRemove)
            {
                jObject.Remove(propertyName);
            }
        }

        public void AddProperty(string key, object value)
        {
            AdditionalProperties[key] = JToken.FromObject(value);
        }

       
    }

    public class BasicInfo
    {
        /// <summary>
        /// 日志的摘要信息
        /// </summary>
        public string? Summary { get; set; }

        /// <summary>
        /// 日志的数字等级，用于控制日志的输出位置
        /// 例如：70-云端，50-本地文件，30-控制台
        /// 一般不用设置 外面设置后写入
        /// </summary>
        public int LogLevelNumber { get; set; }

        /// <summary>
        /// 事件发生的时间戳
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 日志级别，如 INFO, WARN, ERROR 等
        /// </summary>
        public string? LogLevel { get; set; }

        /// <summary>
        /// 主要的日志消息内容
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// 产生日志的主机名
        /// </summary>
        public string? HostName { get; set; }

        /// <summary>
        /// 服务名称 原apiv
        /// </summary>
        public string? ServiceName { get; set; }
             /// <summary>
        /// 服务对象 原apimenu
        /// </summary>
        public string? ServiceMenu { get; set; }
        
          /// <summary>
        /// 服务对象 原apiobj
        /// </summary>
        public string? ServiceObj { get; set; }

   

        /// <summary>
        /// 服务函数 原apifun
        /// </summary>
        public string? ServiceFun { get; set; }

        /// <summary>
        /// 用户唯一标识符
        /// </summary>
        public string? UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string? UserName { get; set; }
        /// <summary>
        /// 日志索引用于elk
        /// </summary>
        public string[]? LogIndex { get; set; }
    }

    public class EventInfo
    {
        /// <summary>
        /// 事件唯一标识符
        /// </summary>
        public string EventId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// 事件类型，如 alert, event, metric, state 等
        /// </summary>
        public string? EventKind { get; set; }

        /// <summary>
        /// 事件类别，如 authentication, database, network 等
        /// </summary>
        public string? EventCategory { get; set; }

        /// <summary>
        /// 具体动作，如 login, logout, purchase 等
        /// </summary>
        public string? EventAction { get; set; }

        /// <summary>
        /// 事件结果，如 success, failure 等
        /// </summary>
        public string? EventOutcome { get; set; }

        /// <summary>
        /// 事件持续时间（毫秒）
        /// </summary>
        public long? EventDuration { get; set; }

        /// <summary>
        /// 事务 ID
        /// </summary>
        public string? TransactionId { get; set; }
    }

    public class ErrorInfo
    {
        /// <summary>
        /// 错误类型
        /// </summary>
        public string? ErrorType { get; set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// 错误堆栈（适用于开发环境）
        /// </summary>
        public string? ErrorStackTrace { get; set; }
    }

    public class HttpInfo
    {
        /// <summary>
        /// HTTP 请求方法
        /// </summary>
        public string? HttpRequestMethod { get; set; }

        /// <summary>
        /// HTTP 请求体内容
        /// </summary>
        public string? HttpRequestBodyContent { get; set; }

        /// <summary>
        /// HTTP 响应状态码
        /// </summary>
        public int? HttpResponseStatusCode { get; set; }

        /// <summary>
        /// 原始 URL
        /// </summary>
        public string? UrlOriginal { get; set; }
    }

    public class TraceInfo
    {
        /// <summary>
        /// 分布式追踪 ID
        /// </summary>
        public string? TraceId { get; set; }

        /// <summary>
        /// 单个追踪中的 span ID
        /// </summary>
        public string? SpanId { get; set; }
    }
}