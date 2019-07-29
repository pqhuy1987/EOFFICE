using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections;

namespace NHG.Logger
{
    public class LogWriter : IDisposable
    {
        StreamWriter writer;
        Log4Net logger;
        String fileName;
        protected static string userID = null;
        protected static string logLevel = "DEBUG";

        public static string LOG_LEVEL
        {
            get { return logLevel; }
            set
            {
                logLevel = value.ToUpper();
            }
        }

        /// <summary>
        /// Get file name of log
        /// </summary>
        public string FileName
        {
            get { return fileName; }
        }
        /// <summary>
        /// IP of client
        /// </summary>
        protected string userIP = null;

        public static int indent = 0;

        #region
        /// <summary>
        /// Constructor.
        /// <br></br>
        /// Su dung file config chung cua domain.
        /// <br></br>
        /// </summary>
        /// <br></br>
        /// <param name="message">name of object</param>
        /// <br></br>
        public LogWriter(string _fileName)
        {
            try
            {
                fileName = _fileName;
                logger = new Log4Net(this.GetType());
                userID = Log4Net.userID;
                userIP = Log4Net.clientIP;
                //Neu truoc do da co mo writer thi phai dong lai
                if (writer != null)
                    Close();
                //Neu folder chua ton tai thi tao folder
                FileInfo f = new FileInfo(fileName);
                if (!Directory.Exists(f.DirectoryName))
                    Directory.CreateDirectory(f.DirectoryName);
                writer = new StreamWriter(fileName, true, Encoding.UTF8);

            }
            catch (Exception ex)
            {
                writer = new StreamWriter(fileName + DateTime.Now.ToString("yyyy-MM-dd"));
                logger.Error(ex);
            }

        }
        /// <summary>
        /// Dong stream
        /// </summary>
        public void Close()
        {
            try
            {
                writer.Close();
            }
            catch (Exception)
            {

            }
        }
        /// <summary>
        /// Ghi buffer xuong file
        /// </summary>
        public void Flush()
        {
            writer.Flush();
        }

        public void Start(object obj)
        {
            Info(">>Open " + obj.ToString());
            indent++;
        }

        public void End(object obj)
        {
            if (indent > 0)
                indent--;
            Info("<<Close " + obj.ToString());

        }
                
        /// <summary>
        /// Tham so ghi log        
        /// </summary>
        /// <param name="name">ten tham so</param>
        /// <param name="value">gia tri tham so</param>
        public void Param(string name, object value)
        {
            if (writer != null)
            {
                Info(name + " = " + value.ToString());
            }
        }

        /// <summary>
        /// ghi log info
        /// <br></br>
        /// </summary>
        /// <param name="obj">The obj.</param>
        public void Info(object obj)
        {
            if (writer != null)
            {
                WriteLog(obj, LogLevel.INFO);
            }
        }

        /// <summary>
        /// mode=Debug         
        /// </summary>
        /// <param name="obj">Object</param>
        public void Debug(object obj)
        {
            if (writer != null)
            {
                WriteLog(obj, LogLevel.DEBUG);
            }
        }

        /// <summary>
        /// Mode Error        
        /// </summary>
        /// <param name="obj">Object</param>
        public void Error(object obj)
        {
            if (writer != null)
            {
                WriteLog(obj, LogLevel.ERROR);

            }
        }
        /// <summary>
        /// Warning
        /// </summary>
        /// <param name="obj">Object</param>
        public void Warning(object obj)
        {
            if (writer != null)
            {
                WriteLog(obj, LogLevel.WARN);
            }
        }


        private void WriteLog(object obj, LogLevel level)
        {
            try
            {
                if (obj is ArrayList)
                {
                    ArrayList arrMessage = (ArrayList)obj;
                    for (int i = 0; i < arrMessage.Count; i++)
                        Write(GetFormDTtring(arrMessage[i].ToString(), level));
                }
                else if (obj is Hashtable)
                {
                    foreach (DictionaryEntry dic in (Hashtable)obj)
                        Param(dic.Key.ToString(), dic.Value.ToString());
                }
                else
                    Write(GetFormDTtring(obj, level));
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }

        }

        private void Write(object value)
        {
            if (value.ToString() != "")
                writer.WriteLine(value);
        }

        private string GetFormDTtring(object message, LogLevel level)
        {
            //Neu muc do log nho hon muc do log duoc dinh nghia thi moi cho phep ghi log
            if (level.GetHashCode() <= GetIntLevel(LOG_LEVEL))
                return DateTime.Now + " " + level.ToString() + " [" + userIP + "]" + "[" + userID + "]" + Space() + message.ToString();
            else
                return "";
        }

        /// <summary>
        /// Tao chuoi ky tu dieu khien.
        /// <br></br>
        /// </summary>
        /// <param name="indent">The indent.</param>
        /// <returns>tab theo cot \t.</returns>
        /// <br></br>
        private string Space()
        {
            string space = "";
            for (int i = 0; i < indent; i++)
            {
                space += "\t";
            }
            return space;
        }

        /// <summary>
        /// Get int value of string level
        /// </summary>
        /// <param name="strLevel">string level</param>
        /// <returns>
        /// DEBUG = 4,
        /// WARN = 3,
        /// INFO = 2,
        /// ERROR = 1
        /// </returns>
        private int GetIntLevel(String strLevel)
        {
            switch (strLevel.ToUpper())
            {
                case "DEBUG": return 4;
                case "WARN": return 3;
                case "INFO": return 2;
                case "ERROR": return 1;
                default:
                    return 0;
            }
        }

        #endregion

        #region fix a violation of this rule.
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
                writer.Close();
            }
            // free native resources
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
