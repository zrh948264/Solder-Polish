using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
namespace Tools
{
    public static class WriteLog
    {
        private static BlockingCollection<string> queue = new BlockingCollection<string>();
        private static AutoResetEvent autoResetEvent = new AutoResetEvent(false);
        private const int RETAIN_LOG_DAY = 7;

        static WriteLog()
        {
            if (!Directory.Exists(@"Logs"))//若文件夹不存在则新建文件夹   
            {
                Directory.CreateDirectory(@"Logs"); //新建文件夹   
            }

            DeleteOldFiles(@"Logs", 30);

            var thread = new Thread(StartConsuming);
            thread.IsBackground = true;
            thread.Start();
        }


        public static void DeleteOldFiles(string dir, int day)
        {
            if (!Directory.Exists(dir) || day < 1) return;
            var now = DateTime.Now;
            foreach (var f in Directory.GetFileSystemEntries(dir).Where(f => File.Exists(f)))
            {
                var t = File.GetCreationTime(f);
                var elapsedTicks = now.Ticks - t.Ticks;
                var elapsedSpan = new TimeSpan(elapsedTicks);

                if (elapsedSpan.TotalDays > day) File.Delete(f);
            }
        }

        public static void AddLog(string message)
        {
            queue.Add(DateTime.Now + "," + message + Environment.NewLine);
            autoResetEvent.Set();
        }

        private static void StartConsuming()
        {
            while (true)
            {
                if (queue.Count > 0)
                {
                    try
                    {
                        File.AppendAllText(@"Logs\" + DateTime.Now.ToString("yyyy-MM-dd") + ".log", queue.Take());
                    }
                    catch
                    { }
                }
                else
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(@"Logs");
                    foreach (var item in directoryInfo.GetFiles("*.log"))
                    {
                        if ((DateTime.Now - item.CreationTime).Days > RETAIN_LOG_DAY)
                        {
                            try
                            {
                                item.Delete();
                            }
                            catch { }
                        }
                    }

                    autoResetEvent.WaitOne();
                }
            }
        }

    }

    public static class WriteForce
    {
        private static BlockingCollection<string> queue = new BlockingCollection<string>();
        private static AutoResetEvent autoResetEvent = new AutoResetEvent(false);
        public static string time = DateTime.Now.Hour + "-" + DateTime.Now.Minute + "-" + DateTime.Now.Second;
        private static string path = @"AllForceData\" + DateTime.Now.ToString("yyyy-MM-dd") + "\\";
        static WriteForce()
        {
            if (!Directory.Exists(@"AllForceData"))//若文件夹不存在则新建文件夹   
            {
                Directory.CreateDirectory(@"AllForceData"); //新建文件夹   
            }

            var thread = new Thread(StartData);
            thread.IsBackground = true;
            thread.Start();
        }


        public static void AddForceData(string aData,string bData)
        {
            queue.Add(DateTime.Now + "," + aData + "," + bData + Environment.NewLine);
            autoResetEvent.Set();
        }

        private static void StartData()
        {
            while (true)
            {
                if (queue.Count > 0)
                {
                    try
                    {
                        if (!Directory.Exists(@"AllForceData\" + DateTime.Now.ToString("yyyy-MM-dd")+"\\"))//若文件夹不存在则新建文件夹   
                        {
                            path = @"AllForceData\" + DateTime.Now.ToString("yyyy-MM-dd") + "\\";
                            Directory.CreateDirectory(path); //新建文件夹   
                        }
                        File.AppendAllText(path + time + ".data", queue.Take());
                    }
                    catch
                    { }
                }
                else
                {
                    autoResetEvent.WaitOne();
                }
            }
        }
    }
}
