﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{

    #region 日志写入类
    /// <summary>
    /// 日志写操作类
    /// 对系统执行过程以及异常进行记录
    /// 写入到本地TXT文档
    /// 文件容量超出2MB字节,删除文件并重新创建
    /// </summary>
    public class LogWriter
    {
        const uint txtContentCapacity = 1; //日志文件的容量,单位MB
        public static string DirectoryBase = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
        public static string ConfigDirectory = DirectoryBase + "Config";
        public static string LogDirectory = DirectoryBase + "SystemLog";
        public static string ExLogFilePath = LogDirectory + "\\" + "ExceptionLogFile.txt";
        public static string SysLogFilePath = LogDirectory + "\\" + "SystemLogFile.txt";

        #region 异常日志
        /// <summary>
        /// 方法：写异常日志
        /// </summary>
        /// <param name="ex"></param>
        public static void WriteException(Exception ex)
        {
            try
            {
                if (System.IO.File.Exists(ExLogFilePath))
                {
                    System.IO.FileInfo _fi = new System.IO.FileInfo(ExLogFilePath);
                    if (_fi.Length > txtContentCapacity * 1024 * 1024)
                    {
                        System.IO.File.Delete(ExLogFilePath);
                        System.IO.File.Create(ExLogFilePath);
                    }
                }

                System.IO.StreamWriter sw = new System.IO.StreamWriter(ExLogFilePath, true, new System.Text.UnicodeEncoding());
                sw.WriteLine(DateTime.Now.ToString("[yy-MM-dd HH:mm:ss]"));
                sw.WriteLine(ex.TargetSite);
                sw.WriteLine(ex.StackTrace);
                sw.WriteLine(ex.Message);
                sw.Flush();
                sw.Close();
            }
            catch { }
        }

        /// <summary>
        /// 方法：根据指定的格式写异常日志
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void WriteException(string format, params object[] args)
        {
            try
            {
                string time = DateTime.Now.ToString("[yy-MM-dd HH:mm:ss]");
                string data = String.Format(format, args);
                string logStr = time + data;
                Write(ExLogFilePath, logStr);
            }
            catch { }
        }
        #endregion

        #region 系统日志

        public static void WriteLog(string format, params object[] args)
        {
            string time = DateTime.Now.ToString("yy-MM-dd HH:mm:ss  ");
            string data = String.Format(format, args);
            string logStr = time + data;
            Write(SysLogFilePath, logStr);
        }

        #endregion

        /// <summary>
        /// 方法：指定路径文件写入内容并检查是否超容量(2*1024*1024)
        /// </summary>
        /// <param name="file"></param>
        /// <param name="content"></param>
        private static void Write(string file, string content)
        {
            try
            {
                if (System.IO.File.Exists(file))
                {
                    System.IO.FileInfo _fi = new System.IO.FileInfo(file);
                    if (_fi.Length > txtContentCapacity * 1024 * 1024)
                    {
                        System.IO.File.Delete(file);

                        System.IO.File.Create(file);
                    }
                }

                System.IO.StreamWriter sw = new System.IO.StreamWriter(file, true, new System.Text.UnicodeEncoding());
                sw.WriteLine(content);
                sw.Flush();
                sw.Close();
            }
            catch { }

        }
    }
    #endregion

}
