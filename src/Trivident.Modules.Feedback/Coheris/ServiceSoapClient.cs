using System;
using System.ServiceModel;

namespace Trivident.Modules.Feedback.Coheris
{
    /// <summary>
    /// Implements correctly disposable WCF client
    /// See https://coding.abel.nu/2012/02/using-and-disposing-of-wcf-clients/
    /// </summary>
    public partial class ServiceSoapClient : IDisposable
    {
        #region IDisposable implementation

        /// <summary>
        /// IDisposable.Dispose implementation, calls Dispose(true).
        /// </summary>
        void IDisposable.Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Dispose worker method. Handles graceful shutdown of the
        /// client even if it is an faulted state.
        /// </summary>
        /// <param name="disposing">Are we disposing (alternative
        /// is to be finalizing)</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                try
                {
                    if (State != CommunicationState.Faulted)
                    {
                        Close();
                    }
                }
                finally
                {
                    if (State != CommunicationState.Closed)
                    {
                        Abort();
                    }
                }
            }
        }

        /// <summary>
        /// Finalizer.
        /// </summary>
        ~ServiceSoapClient()
        {
            Dispose(false);
        }

        #endregion
    }
}