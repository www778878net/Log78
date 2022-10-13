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
            //�����־
            //Logger.Debug("�Ҵ����Nlog��־7788��");
            //Logger.Info("�Ҵ����Nlog��־7788��");
            //Logger.Warn("�Ҵ����Nlog��־7788��");
            //Logger.Error("�Ҵ����Nlog��־7788��");

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
            //�����־
            ltest.Debug("�Ҵ����7788��־7788��");
            ltest.Info("�Ҵ����7788��־7788��");
            ltest.Warn("�Ҵ����7788��־7788��");
            ltest.Error("�Ҵ����7788��־7788��");

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
            //�����־
            ltest.Debug("�Ҵ����7788��־7788��");
            ltest.Info("�Ҵ����7788��־7788��");
            ltest.Warn("�Ҵ����7788��־7788��");
            ltest.Error("�Ҵ����7788��־7788��");

            int test = 1;//nothing to test
            Assert.AreEqual(1, test);
        }

        public void TestMethod1()
        {
            //����һ�������ļ�����
            var config = new NLog.Config.LoggingConfiguration();
            //������־д��Ŀ�ĵ�
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
            //�����־·�ɹ���
            config.AddRule(LogLevel.Trace, LogLevel.Fatal, logfile, "Microsoft.*", true);
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);
      

            //�����ļ���Ч
            LogManager.Configuration = config;
            
            //������־��¼����ʽ1
            Logger Logger = LogManager.GetCurrentClassLogger();
            //������־��¼����ʽ2���ֶ�����
            Logger Logger2 = LogManager.GetLogger("MyLogger");
            //�����־
            Logger.Debug("�Ҵ����Nlog��־323��");
 
        }
    }
}