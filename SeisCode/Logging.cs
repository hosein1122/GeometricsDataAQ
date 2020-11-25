using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SeisCode
{
 public  class Logger
    {
        private enum Severity
        {
            Error,
            Exception,
            Info,
            Warning  
        }
        private static readonly object Lock = new object();
        private const string Path = "Log";

        #region public Methods
        /// <summary>
        /// شروع برنامه را در لاگ مینویسد
        /// </summary>
        /// <param name="prefix">پیشوند فایل لاگ</param>
        public static void InfoStart(string prefix = "L")
        {
            new Task(() => DoLog(prefix, DateTime.UtcNow)).Start();
        }

        /// <summary>
        /// Info ایجاد لاگ از نوع
        /// </summary>
        /// <param name="_text">متن پیام</param>
        /// <param name="prefix">پیشوند فایل لاگ</param>
        public static void Info(string _text, string prefix = "L")
        {
            new Task(() => DoLog(_text, prefix, Severity.Info, DateTime.UtcNow)).Start();
        }

        /// <summary>
        ///Warning ایجاد لاگ از نوع
        /// </summary>
        /// <param name="_text">متن پیام</param>
        /// <param name="prefix">پیشوند فایل لاگ</param>
        public static void Warning(string _text, string prefix = "L")
        {
            new Task(() => DoLog(_text, prefix, Severity.Warning, DateTime.UtcNow)).Start();
        }

        /// <summary>
        ///Error ایجاد لاگ از نوع
        /// </summary>
        /// <param name="_text">متن پیام</param>
        /// <param name="prefix">پیشوند فایل لاگ</param>
        public static void Error(string _text, string prefix = "L")
        {
            new Task(() => DoLog(_text, prefix, Severity.Error, DateTime.UtcNow)).Start();
        }
        
        /// <summary>
        /// ایجاد لاگ با جزئیات از خطای رخداده
        /// </summary>
        /// <param name="ex">خطای برنامه</param>
        /// <param name="prefix">پیشوند فایل لاگ</param>
        public static void Log(Exception ex, string prefix = "L")
        {
            new Task(() => DoLog(ex, prefix, Severity.Exception, DateTime.UtcNow)).Start();
        }
        #endregion


        #region Private Functions
        private static void DoLog(string prefix, DateTime dt)
        {
            if (Monitor.TryEnter(Lock, 5000))
            {
                try
                {
                    if (!Directory.Exists(Path))
                        Directory.CreateDirectory(Path);

                    using (StreamWriter writer = new StreamWriter(Path + "\\" + prefix + "_" + dt.ToString("yyyyMMdd") + ".txt", true))
                    {
                        writer.WriteLine();
                        writer.WriteLine("=============================================");
                        writer.WriteLine("{0}  {1,-10}  {2}",
                        dt.ToString("HH:mm:ss.fff"), Severity.Info.ToString(), "Program Started");
                    }
                }
                finally
                {
                    Monitor.Exit(Lock);
                }
            }
        }
        private static void DoLog(string _text, string prefix, Severity _severity, DateTime dt)
        {
            if (Monitor.TryEnter(Lock, 5000))
            {
                try
                {
                    if (!Directory.Exists(Path))
                        Directory.CreateDirectory(Path);

                    using (StreamWriter writer = new StreamWriter(Path + "\\" + prefix + "_" + dt.ToString("yyyyMMdd") + ".txt", true))
                    {
                        writer.WriteLine("{0}  {1,-10}  {2}",
                        dt.ToString("HH:mm:ss.fff"), _severity.ToString(), _text);
                    }
                }
                finally
                {
                    Monitor.Exit(Lock);
                }
            }
        }
        private static void DoLog(Exception ex, string prefix, Severity _severity, DateTime dt)
        {
            if (Monitor.TryEnter(Lock,5000))
            {
                try
                {
                    if (!Directory.Exists(Path))
                        Directory.CreateDirectory(Path);

                    using (StreamWriter writer = new StreamWriter(Path + "\\" + prefix + "_" + dt.ToString("yyyyMMdd") + ".txt", true))
                    {
                        //ex.Source    نام برنامه اصلی
                        //ex.TargetSite   نام و نوع تابع صادر کننده خطا
                        //ex.TargetSite.Name  نام تابع صادر کننده خطا
                        //ex.TargetSite.DeclaringType مسیر فرواخوانی تابع
                        //ex.TargetSite.DeclaringType.FullName  مسیر کامل فرواخوانی تابع
                        writer.WriteLine("{0}  {1,-10}  Message:  {2}\r\n{3,-26}Path:  {4}\r\n{3,-26}ThreadId: {5} ",
                        dt.ToString("HH:mm:ss.fff"), _severity.ToString(), ex.Message,
                        "",
                        ex.StackTrace, Thread.CurrentThread.ManagedThreadId);
                    }
                }
                finally
                {
                    Monitor.Exit(Lock);
                }
            }
        }
        #endregion
    }

 
}
