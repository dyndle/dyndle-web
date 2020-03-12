using DD4T.ContentModel.Contracts.Logging;
using Trivident.Modules.Logging.Enterprise;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trivident.Modules.Logging
{
    /// <summary>
    /// Logger implementation used to wrap the provided logger into the common ILogger interface from DD4T.
    /// </summary>
    /// <seealso cref="DD4T.ContentModel.Contracts.Logging.ILogger" />
    public class LogWrapper : ILogger
    {
        public void Critical(string message, LoggingCategory category, params object[] parameters)
        {
            DefaultLogger.Write(nameof(this.Critical), message.FormatString(parameters));
        }

        public void Critical(string message, params object[] parameters)
        {
            DefaultLogger.Write(nameof(this.Critical), message.FormatString(parameters));
        }

        public void Debug(string message, LoggingCategory category, params object[] parameters)
        {
            DefaultLogger.Write(nameof(this.Debug), message.FormatString(parameters));
        }

        public void Debug(string message, params object[] parameters)
        {
            DefaultLogger.Write(nameof(this.Debug), message.FormatString(parameters));
        }

        public void Error(string message, LoggingCategory category, params object[] parameters)
        {
            DefaultLogger.Write(nameof(this.Error), message.FormatString(parameters));
        }

        public void Error(string message, params object[] parameters)
        {
            DefaultLogger.Write(nameof(this.Error), message.FormatString(parameters));
        }

        public void Information(string message, LoggingCategory category, params object[] parameters)
        {
            DefaultLogger.Write(nameof(this.Information), message.FormatString(parameters));
        }

        public void Information(string message, params object[] parameters)
        {
            DefaultLogger.Write(nameof(this.Information), message.FormatString(parameters));
        }

        public void Warning(string message, LoggingCategory category, params object[] parameters)
        {
            DefaultLogger.Write(nameof(this.Warning), message.FormatString(parameters));
        }

        public void Warning(string message, params object[] parameters)
        {
            DefaultLogger.Write(nameof(this.Warning), message.FormatString(parameters));
        }
    }
}