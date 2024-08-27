using System;
using System.IO;
using System.Xml.Serialization;



namespace www778878net.log
{
    public class FileLog78:IFileLog78
    {
        public string menu { get; set; }

        private string file;
        private static object olook = new object();
        public static string logpath{get;set;} = "/";

        public FileLog78(string _menu) 
        { 
            this.menu = _menu;
            int idate = DateTime.Now.Day % 3;
            file = menu + idate.ToString() + ".txt";
            Clear();
        }
        public void LogToFile(string message)
        {
            lock (olook)
            {
                try
                {
                    File.AppendAllText( file, message);
                }
                catch { }
            }
        }

        /// <summary>
        /// 默认清除LOG只保留3天的
        /// </summary>
        public void Clear()
        {
            int idate = DateTime.Now.Day % 3;
            for (int i = 0; i < 3; i++)
            {
                if (i == idate) continue;
                File.Delete(menu + i.ToString() + ".txt");
            }

        }
    }
}