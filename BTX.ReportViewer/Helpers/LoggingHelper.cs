using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Text;
using System.Text.RegularExpressions;
using log4net;

namespace BTX.ReportViewer.Helpers
{
    public class LoggingHelper
    {
        #region Private Member
        private static readonly ILog logger = LogManager.GetLogger(logAppender);
        #endregion

        #region Constants
        private const string auditStoredProcedure = "usp_InsertDataLogHistory";
        private const string logAppender = "FileAppender";
        #endregion

        #region Methods

        public static void LogException(Exception ex)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                logger.Fatal(ex.Message, ex);
            }
            catch
            {
            }
        }
        public static void LogException(Exception ex, string sproc, Dictionary<string, object> parameters)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                StringBuilder details = new StringBuilder();
                details.AppendFormat("Message: {0}. Procedure: {1}", ex.Message, sproc);
                foreach (string key in parameters.Keys)
                {
                    details.AppendFormat("\r\n\tParam: '{0}'. Value: '{1}'", key, parameters[key]);
                }
                logger.Fatal(details.ToString(), ex);
            }
            catch
            {
            }
        }
        public static void LogException(string msg, Exception ex)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                logger.Fatal(msg, ex);
            }
            catch
            {
            }
        }

        public static void LogError(string msg)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                logger.Error(msg);
            }
            catch
            {
            }
        }

        public static void LogTrace(string msg)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                logger.Info(msg);
            }
            catch
            {
            }
        }

        public static void LogInformation(string msg)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                logger.Info(msg);
            }
            catch
            {
            }
        }

        public static void LogDebug(string msg)
        {
            try
            {
                bool enableVerboseDebugging = false;
                bool.TryParse(ConfigurationManager.AppSettings["EnableVerboseDebugging"], out enableVerboseDebugging);
                if (enableVerboseDebugging)
                {
                    log4net.Config.XmlConfigurator.Configure();
                    logger.Debug(msg);
                }
            }
            catch
            {
            }
        }

        public static void LogDebug(string className, string method, string msg)
        {
            try
            {
                bool enableVerboseDebugging = false;
                bool.TryParse(ConfigurationManager.AppSettings["EnableVerboseDebugging"], out enableVerboseDebugging);
                if (enableVerboseDebugging)
                {
                    log4net.Config.XmlConfigurator.Configure();
                    logger.Debug(string.Format("{0}.{1}: {2}", className, method, msg));
                }
            }
            catch
            {
            }
        }

        
        #endregion
    }
}