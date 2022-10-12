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
            //�����־
            Logger.Debug("�Ҵ����Nlog��־7788��");
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