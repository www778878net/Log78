using NLog;
using www778878net.Log;
namespace www778878net.Test
{
    [TestClass]
    public class Test78
    {
        [TestMethod]
        public void Test()
        {
            Log78.Debug("test333");
            Logger Logger = LogManager.GetCurrentClassLogger(); 
            //打出日志
            Logger.Debug("我打出了Nlog日志7788！");
        }
     
        public void TestMethod1()
        {
            //创建一个配置文件对象
            var config = new NLog.Config.LoggingConfiguration();
            //创建日志写入目的地
            var logfile = new NLog.Targets.FileTarget("logfile") {
                FileName = "logs/log.txt",
                Layout = "${longdate} [${level}].[${logger}].[${threadid}}].[${elapse}]${newline}${message}${newline}${onexception:inner=${newline} *****Error***** ${newline} ${exception:format=toString}${exception:format=StackTrace}}",
                ArchiveAboveSize = 104857600,
                ArchiveEvery = NLog.Targets.FileArchivePeriod.Day,
                ArchiveNumbering =NLog.Targets.ArchiveNumberingMode.Rolling,  // "Rolling",
                ArchiveFileName = "logs/archives/log.{###}.txt",
                ArchiveDateFormat = "yyyyMMdd-hhmmss",
                MaxArchiveFiles = 30,
                EnableFileDelete = true

            };
            //添加日志路由规则
            config.AddRule(LogLevel.Trace, LogLevel.Fatal, logfile, "Microsoft.*", true);
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);
      

            //配置文件生效
            LogManager.Configuration = config;
            
            //创建日志记录对象方式1
            Logger Logger = LogManager.GetCurrentClassLogger();
            //创建日志记录对象方式2，手动命名
            Logger Logger2 = LogManager.GetLogger("MyLogger");
            //打出日志
            Logger.Debug("我打出了Nlog日志323！");
 
        }
    }
}