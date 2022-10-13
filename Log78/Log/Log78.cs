using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace www778878net.Log
{
    /// <summary>
    /// 封装log 方便使用和切换
    /// </summary>
    public   class Log78
    {
        public  delegate void LogHandler(string leave,string info);
        public   event LogHandler? EventLog;
        public Logger Logger;
        /// <summary>
        /// 单例
        /// </summary>
        public static Log78 client;
        static Log78()
        {
            //创建一个配置文件对象
            var config = new NLog.Config.LoggingConfiguration();
            //创建日志写入目的地
            var logfile = new NLog.Targets.FileTarget("logfile")
            {
                FileName = "logs/log.txt",
                Layout = "${longdate} [${level}].[${logger}].[${threadid}}].[${elapse}]${newline}${message}${newline}${onexception:inner=${newline} *****Error***** ${newline} ${exception:format=toString}${exception:format=StackTrace}}",
                ArchiveAboveSize = 104857600,
                ArchiveEvery = NLog.Targets.FileArchivePeriod.Day,
                ArchiveNumbering = NLog.Targets.ArchiveNumberingMode.Rolling,  // "Rolling",
                ArchiveFileName = "logs/archives/log.{###}.txt",
                ArchiveDateFormat = "yyyyMMdd-hhmmss",
                MaxArchiveFiles = 10,
                EnableFileDelete = true

            };
            //添加日志路由规则
            config.AddRule(LogLevel.Trace, LogLevel.Fatal, logfile, "Microsoft.*", true);
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);

            //配置文件生效
            LogManager.Configuration = config;
            client = new  ();
            
        }

        public Log78()
        {
            Logger = LogManager.GetCurrentClassLogger();
        }

        private   void OnEventLog(string leave, string message)
        {
            if (EventLog != null)
            {
                EventLog(leave,message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public   void Debug(string info)
        {  
            //打出日志
            Logger.Debug(info);
        }

        /// <summary>
        /// 
        /// </summary>
        public   void Info(string info)
        {
            //打出日志
            Logger.Info(info);
            OnEventLog("Info", info);
        }

        /// <summary>
        /// 
        /// </summary>
        public   void Warn(string info)
        {
            //打出日志
            Logger.Warn(info);
            OnEventLog("Warn", info);
        }

        /// <summary>
        /// 
        /// </summary>
        public   void Error(string info)
        {
            //打出日志
            Logger.Error(info);
            OnEventLog("Error", info);
        }

        /// <summary>
        /// 
        /// </summary>
        public   void Error(Exception err)
        {
            //打出日志
            Logger.Error(err);
            OnEventLog("Error", err.Message);
        }
    }
}
