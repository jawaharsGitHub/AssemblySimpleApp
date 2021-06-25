using log4net;
using log4net.Config;

namespace Common
{
    public class LogHelper
    {
        private readonly ILog _debugLogger;
        private string LogPath;
        private static ILog GetLogger(string logName)
        {
            ILog log = LogManager.GetLogger(logName);
            return log;
        }

        private  LogHelper()
        {

        }

        public LogHelper(string logName, string filePath = null)
        {
            //logger names are mentioned in <log4net> section of config file
            _debugLogger = GetLogger(logName);
            //GlobalContext.Properties["LogFileName"] = filePath; //log file path 
            XmlConfigurator.Configure();
        }

        /// <summary>
        /// This method will write log in Log_USERNAME_date{yyyyMMdd}.log file
        /// </summary>
        /// <param name="message"></param>
        public void WriteLog(string message, int? customerId = null, int? customerSeqId = null)
        {
            _debugLogger.DebugFormat($"[{customerId}-{customerSeqId}] {message}");
        }

        public void WriteAdangalLog(string message)
        {
            _debugLogger.DebugFormat($"{message}");
        }

       
    }
}
