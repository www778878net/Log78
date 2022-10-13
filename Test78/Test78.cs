using NLog;
using www778878net.Log;
namespace www778878net.Test
{
    [TestClass]
    public class Test78
    {
        [TestMethod]
        public void TestStatic()
        {
            Log78.client.Debug("test333");
           // Logger Logger = LogManager.GetCurrentClassLogger();
            Log78.client.EventLog += Log78_EventLog;
            //打出日志
            //Logger.Debug("我打出了Nlog日志7788！");
            //Logger.Info("我打出了Nlog日志7788！");
            //Logger.Warn("我打出了Nlog日志7788！");
            //Logger.Error("我打出了Nlog日志7788！");

            int test = 1;//nothing to test
            Assert.AreEqual(1, test);
        }

        private void Log78_EventLog(string leave, string info)
        {
             //do something

        }

        [TestMethod]
        public void Testclass()
        {
            Log78 ltest = new();
            ltest.Debug("test7788");
    
            Log78.client.EventLog += Log78_EventLog;
            //打出日志
            ltest.Debug("我打出了7788日志7788！");
            ltest.Info("我打出了7788日志7788！");
            ltest.Warn("我打出了7788日志7788！");
            ltest.Error("我打出了7788日志7788！");

            int test = 1;//nothing to test
            Assert.AreEqual(1, test);
        }

        [TestMethod]
        public void Testthisclass()
        {
            Log78 ltest = new();
            ltest.Logger= LogManager.GetCurrentClassLogger();
            ltest.Debug("test7788 thisclass");

            Log78.client.EventLog += Log78_EventLog;
            //打出日志
            ltest.Debug("我打出了7788日志7788！");
            ltest.Info("我打出了7788日志7788！");
            ltest.Warn("我打出了7788日志7788！");
            ltest.Error("我打出了7788日志7788！");

            int test = 1;//nothing to test
            Assert.AreEqual(1, test);
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