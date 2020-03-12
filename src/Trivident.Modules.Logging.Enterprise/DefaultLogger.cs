using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Trivident.Modules.Logging.Enterprise
{
    /// <summary>
    ///The Logger class is used to define the methods for write log with given parameter
    /// </summary>
    /// <Name>Prasanna Kumar</Name>
    /// <CreatedOn>June 14, 2016</CreatedOn> 
    ///
    public class DefaultLogger
    {
        private static LogEntry objTraceLogEntry = null;  
        private static LogWriterFactory logWriterFactory = null;
        private static LogWriter logwriter = null;

        static DefaultLogger()
        {
            //DatabaseFactory.SetDatabaseProviderFactory(new DatabaseProviderFactory(), false);
            IConfigurationSource config = ConfigurationSourceFactory.Create();

            logWriterFactory = new LogWriterFactory(config);
            //Logger.SetLogWriter(logWriterFactory.Create(), false);
            logwriter = logWriterFactory.Create();
            Microsoft.Practices.EnterpriseLibrary.Logging.Logger.SetLogWriter(logwriter, false);
            var exceptionpolicyfactory = new ExceptionPolicyFactory(config);
            ExceptionPolicy.SetExceptionManager(exceptionpolicyfactory.CreateManager(), false);
        }

        /// <summary>
        /// This method will write information in log file with severity decided based on the parameters passed to it
        /// Title and Message are mandatory params & if exception param is passed then it will log in error mode
        /// Title and Message are mandatory params & if exception param is null and Dictionary<String, Object> is passed then it will log in Verbose mode
        /// Title and Message are mandatory params & if exception param is null and Dictionary<String, Object> is null then it will log in Information mode
        /// </summary>
        /// <param name="message"></param>
        /// <param name="title"></param>
        /// <param name="exception"></param>

        public static void Write(string title, string message, Dictionary<String, Object> extendedProperties = null, Exception ex = null)
        {
            //logwriter = logWriterFactory.Create();
            //Microsoft.Practices.EnterpriseLibrary.Logging.Logger.SetLogWriter(logwriter, false);
            objTraceLogEntry = new LogEntry();
            try
            {
                if (ex != null)
                {
                    objTraceLogEntry.Categories.Clear();
                    objTraceLogEntry.Categories.Add("Error");
                    objTraceLogEntry.Title = title;
                    objTraceLogEntry.Message = message;
                    objTraceLogEntry.Severity = TraceEventType.Critical;
                    objTraceLogEntry.TimeStamp = System.DateTime.Now;
                    logwriter.Write(objTraceLogEntry);
                    objTraceLogEntry.Message = ex.StackTrace;
                    if (extendedProperties != null)
                    {
                        objTraceLogEntry.ExtendedProperties = extendedProperties;
                    }
                    else
                    {
                        objTraceLogEntry.ExtendedProperties = null;
                    }
                    logwriter.Write(objTraceLogEntry);
                }
                else if (extendedProperties != null)
                {
                    objTraceLogEntry.Categories.Clear();
                    objTraceLogEntry.Categories.Add("Verbose");
                    objTraceLogEntry.Title = title;
                    objTraceLogEntry.Message = message;
                    objTraceLogEntry.TimeStamp = System.DateTime.Now;
                    objTraceLogEntry.Severity = TraceEventType.Verbose;
                    objTraceLogEntry.ExtendedProperties = extendedProperties;
                    logwriter.Write(objTraceLogEntry);
                }
                else
                {
                    objTraceLogEntry.Categories.Clear();
                    objTraceLogEntry.Categories.Add("Information");
                    objTraceLogEntry.Title = title;
                    objTraceLogEntry.Message = message;
                    objTraceLogEntry.Severity = TraceEventType.Information;
                    objTraceLogEntry.TimeStamp = System.DateTime.Now;
                    objTraceLogEntry.ExtendedProperties = extendedProperties;
                    logwriter.Write(objTraceLogEntry);
                }
            }
            catch (Exception exa)
            {
                objTraceLogEntry.Message = exa.StackTrace;
                objTraceLogEntry.Severity = TraceEventType.Critical;
                objTraceLogEntry.Categories.Add("Error");
                logwriter.Write(objTraceLogEntry);
            }
            finally
            {
                // logwriter = null;
            }
        }

        public static void Exception(Exception e, string policy)
        {
            ExceptionPolicy.HandleException(e, policy);
        }
    }
}