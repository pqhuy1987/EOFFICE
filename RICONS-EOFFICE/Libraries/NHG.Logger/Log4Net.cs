using log4net;
using System;
using System.Collections;
using System.IO;

namespace NHG.Logger
{
    /// <summary>
    /// LogLevel of log4net and log stream
    /// </summary>
    public enum LogLevel
    {
        DEBUG = 4,
        WARN = 3,
        INFO = 2,
        ERROR = 1

    };

    /// <summary>
    /// Class for logging.
    /// <br></br>
    /// Khi su dung can dinh nghia file app.config hoac web.config de ghi log.
    /// <br></br>
    /// </summary>
    public partial class Log4Net
    {

        #region
        /// <summary>
        /// Log4net
        /// </summary>
        private readonly ILog logger;
        /// <summary>
        /// (UserID !=null?UserID:userID)
        /// </summary>
        public static string userID = null;
        /// <summary>
        /// IP cua may dang chay (xuat log cho client)
        /// </summary>
        public static string clientIP = null;

        public static string macaddress = null;

        public static string browser = null;

        public static string useragent = null;

        public static string countryname = null;

        protected string _userID = null;

        protected static String logLevel = "";
        /// <summary>
        /// Get/Set the logLevel of Application.
        /// List of LogLevel : DEBUG, INFO , WARN , ERROR
        /// </summary>
        public static String LOG_LEVEL
        {
            get
            {
                return logLevel;
            }

            set
            {
                if (value != null && value != "")
                    logLevel = value.ToUpper();

            }
        }
        /// <summary>
        /// Set the log level of log4net by RuntimeLogLevlel (not depend on level in xml config file)
        /// Level Default : DEBUG
        /// </summary>
        protected static void ResetLogLevel()
        {
            switch (logLevel)
            {
                case "DEBUG":
                    ((log4net.Repository.Hierarchy.Hierarchy)LogManager.GetRepository()).Root.Level = log4net.Core.Level.Debug;
                    break;
                case "INFO":
                    ((log4net.Repository.Hierarchy.Hierarchy)LogManager.GetRepository()).Root.Level = log4net.Core.Level.Info;
                    break;
                case "WARN":
                    ((log4net.Repository.Hierarchy.Hierarchy)LogManager.GetRepository()).Root.Level = log4net.Core.Level.Warn;
                    break;
                case "ERROR":
                    ((log4net.Repository.Hierarchy.Hierarchy)LogManager.GetRepository()).Root.Level = log4net.Core.Level.Error;
                    break;
            }
        }

        public String UserID
        {
            get { return _userID; }
            set { _userID = value; }
        }



        /// <summary>
        /// IP of client truyen len service
        /// </summary>
        public string userIP = null;


        ///<sumary>
        /// 
        /// <br></br>
        /// DT.CommonLib.DTLogger.indent
        ///</sumary>       

        public static int indent = 0;
        /// <summary>
        /// AKS.CommonLib.ResourceUtility [KIDS Project]
        /// </summary>
        public static string defaultConfigPath = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
        public static string currentConfigPath = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;

        #endregion Properties

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
        public Log4Net(string message)
            : this(message, currentConfigPath)
        {

        }
        /// <summary>
        /// Constructor.
        /// <br></br>
        /// Su dung file config chung cua domain.
        /// <br></br>
        /// </summary>
        /// <param name="type">type of object</param>
        /// <br></br>
        public Log4Net(Type type)
            : this(type, defaultConfigPath)
        {

        }

        /// <summary>
        /// Constructor.
        /// <br></br>
        /// Su dung khi muon chi dinh file log.config duoc dinh nghia rieng.
        /// <br></br>
        /// </summary>
        /// <param name="message">object logged</param>
        /// <param name="str_configPath">The str_config path "../../DT/CommonLib/log.config"</param>
        /// <br></br>
        /// <br></br>
        public Log4Net(string message, string str_configPath)
        {
            try
            {
#if DEBUG
                //lay thong tin confilg cho logger tu file config.
                System.IO.FileInfo fileInfo = new System.IO.FileInfo(str_configPath);
                log4net.Config.XmlConfigurator.Configure(fileInfo);

#else
                String _configName = "LogConfig.config";
                if(str_configPath != defaultConfigPath)
                                {
                                    System.Net.WebClient client = new System.Net.WebClient();              
                                    client.DownloadFile(str_configPath,_configName);
                                    System.IO.FileStream stream = new System.IO.FileStream(_configName, FileMode.Open, FileAccess.Read); 
                                    log4net.Config.XmlConfigurator.Configure(stream);
                                    stream.Close();
                                }
                                else
                                {
                                     System.IO.FileInfo fileInfo = new System.IO.FileInfo(str_configPath);
                                    log4net.Config.XmlConfigurator.Configure(fileInfo);
                                }
#endif
                ////lay thong tin confilg cho logger tu file config.

                //System.IO.FileInfo fileInfo = new System.IO.FileInfo(str_configPath);

                //if(fileInfo!=null)
                //    log4net.Config.XmlConfigurator.Configure(fileInfo);
                ResetLogLevel();
                logger = LogManager.GetLogger(message);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }
        /// <summary>
        /// Constructor.
        /// <br></br>
        /// Su dung khi muon chi dinh file log.config duoc dinh nghia rieng.
        /// <br></br>
        /// </summary>
        /// <param name="type">type of object</param>
        /// <param name="str_configPath">The str_config path.</param>
        /// <br></br>
        /// <br></br>
        public Log4Net(Type type, string str_configPath)
        {
            try
            {
#if DEBUG
                //lay thong tin confilg cho logger tu file config.
                System.IO.FileInfo fileInfo = new System.IO.FileInfo(str_configPath);
                log4net.Config.XmlConfigurator.Configure(fileInfo);

#else
                String _configName = "LogConfig.config";
                if(str_configPath != defaultConfigPath)
                {
                    System.Net.WebClient client = new System.Net.WebClient();              
                    client.DownloadFile(str_configPath,_configName);
                    System.IO.FileStream stream = new System.IO.FileStream(_configName, FileMode.Open, FileAccess.Read); 
                    log4net.Config.XmlConfigurator.Configure(stream);
                    stream.Close();
                }
                else
                {
                     System.IO.FileInfo fileInfo = new System.IO.FileInfo(str_configPath);
                    log4net.Config.XmlConfigurator.Configure(fileInfo);
                }
#endif
                ResetLogLevel();
                logger = LogManager.GetLogger(type.ToString());
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }
        #endregion

        #region Methods

        public static void SetConfigPath(string _configPath)
        {
            currentConfigPath = _configPath;
        }

        public static void SetDefaultConfigPath()
        {
            SetConfigPath(defaultConfigPath);
        }
        /// <summary>
        /// 
        /// <br></br>
        /// </summary>
        /// <param name="obj">The obj.</param>
        public void Start(object obj)
        {
            if (logger != null)
            {
                logger.Info("[" + ((userIP != null ? userIP : clientIP)) + " - " + ((browser != null ? browser : browser)) + "][" + (UserID != null ? UserID : userID) + "]" + Space(indent) + ">> Init " + obj + " >>");
                indent++;
            }
        }

        /// <summary>
        /// 
        /// <br></br>
        /// </summary>
        /// <param name="obj">The obj.</param>
        public void End(object obj)
        {
            if (logger != null)
            {
                indent--;
                logger.Info("[" + ((userIP != null ? userIP : clientIP)) + " - " + ((browser != null ? browser : browser)) + "][" + ((UserID != null ? UserID : userID)) + "]" + Space(indent) + "<< End " + obj + " <<");
            }
        }

        /// <summary>
        ///
        /// <br></br>
        /// </summary>
        /// <param name="indent">The indent.</param>
        /// <returns>string </returns>
        /// <br></br>
        private string Space(int indent)
        {
            string space = "";
            for (int i = 0; i < indent; i++)
            {
                space += "\t";
            }
            return space;
        }
        /// <summary>
        ///
        /// <br></br>
        /// </summary>
        /// <param name="obj">The obj.</param>
        public void Info(object obj)
        {
            if (logger != null)
            {
                logger.Info("[" + ((userIP != null ? userIP : clientIP)) + " - " + ((browser != null ? browser : browser)) + "][" + (UserID != null ? UserID : userID) + "] " + Space(indent) + obj);
            }
        }
        /// <summary>
        ///     
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void Param(string name, object value)
        {
            if (logger != null)
            {
                logger.Info("[" + ((userIP != null ? userIP : clientIP)) + " - " + ((browser != null ? browser : browser)) + "][" + (UserID != null ? UserID : userID) + "] " + Space(indent) + name + "= " + value);
            }
        }
        /// <summary>
        /// mode=Debug ‚ÌÛAƒƒO‚ð‹L˜^‚·‚éBmode != Debug ‚Ìê‡‚Í‹L˜^‚µ‚È‚¢.         
        /// </summary>
        /// <param name="obj">Object</param>

        public void Debug(object obj)
        {
            if (logger != null)
            {
                if (obj is Hashtable)
                    LogHashTable(obj as Hashtable);
                else
                    logger.Debug("[" + ((userIP != null ? userIP : clientIP)) + " - " + ((browser != null ? browser : browser)) + "][" + (UserID != null ? UserID : userID) + "] " + Space(indent) + obj);
            }
        }
        /// <summary>
        /// ƒGƒ‰[ƒƒO‚ð‹L˜^‚·‚é.        
        /// </summary>
        /// <param name="obj">Object</param>
        public void Error(object obj)
        {
            if (logger != null)
            {
                Log(obj, "Error");
                //logger.Error("[" + userIP + "][" + (UserID !=null?UserID:userID) + "] Error : " + Space(indent) + obj);
            }
        }
        /// <summary>
        /// ŒxƒƒO‚ð‹L˜^‚·‚é.
        /// </summary>
        /// <param name="obj">Object</param>
        public void Warning(object obj)
        {
            if (logger != null)
            {
                logger.Warn("[" + ((userIP != null ? userIP : clientIP)) + " - " + ((browser != null ? browser : browser)) + "][" + (UserID != null ? UserID : userID) + "] Warning : " + Space(indent) + obj);
            }
        }
        /// <summary>
        /// ƒCƒ“ƒfƒ“ƒg=@0 ‚ÉƒŠƒZƒbƒg‚·‚é.
        /// <br></br>
        /// </summary>
        public static void ResetIndent()
        {
            Log4Net.indent = 0;
        }

        /// <summary>
        /// Ghi log cho hashtable
        /// </summary>
        /// <param name="parameter"></param>
        private void LogHashTable(Hashtable parameter)
        {
            try
            {
                if (parameter == null)
                {
                    Debug("parameter null");
                    return;
                }

                foreach (DictionaryEntry dic in (Hashtable)parameter)
                    Param(dic.Key.ToString(), dic.Value.ToString());
            }
            catch (Exception)
            {
            }
        }

        private void Log(object obj, string level)
        {
            if (obj is ArrayList)
            {
                ArrayList arrMessage = (ArrayList)obj;
                for (int i = 0; i < arrMessage.Count; i++)
                    logger.Error("[" + ((userIP != null ? userIP : clientIP)) + " - " + ((browser != null ? browser : browser)) + "][" + (UserID != null ? UserID : userID) + "] " + level + " : " + Space(indent) + arrMessage[i].ToString());
            }
            else
                logger.Error("[" + ((userIP != null ? userIP : clientIP)) + " - " + ((browser != null ? browser : browser)) + "][" + (UserID != null ? UserID : userID) + "] " + level + " : " + Space(indent) + obj.ToString());

        }
        #endregion
    }
}
