using System;
using DD4T.ContentModel.Contracts.Logging;
using Dyndle.Modules.Core.Extensions;
using Trivident.Modules.Feedback.Coheris;
using Trivident.Modules.Feedback.Contracts;
using Trivident.Modules.Feedback.Handlers;
using Trivident.Modules.Feedback.Models;

namespace Trivident.Modules.Feedback.Services
{
    /// <summary>
    /// Send feedback form data to Coheris using SOAP client
    /// </summary>
    public class CoherisFeedbackFormSenderService : IFeedbackFormSenderService
    {
        private readonly ILogger _logger;

        /// <summary>
        /// Injected dependencies
        /// </summary>
        /// <param name="logger"></param>
        public CoherisFeedbackFormSenderService(ILogger logger)
        {
            logger.ThrowIfNull(nameof(logger));
            _logger = logger;
        }

        /// <summary>
        /// Send for data to Coheris using SOAP client
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public SendResult SendData(int siteId, FeedbackFormData data)
        {
            using (var client = new ServiceSoapClient())
            {
                var xml = client.addContact(data.Title, data.Name, data.FirstName, data.Address, data.City, data.PostalCode, data.Phone, data.Country, data.Email, data.AgeRange, data.AffectedProduct, data.ControlNumber, data.BarCode, data.Message, data.Reason, siteId, data.NewsletterOptIn);
                _logger.Information($"Result of Coheris addContact for site ID {siteId} => {xml}");
                return new CoherisResponseHandler(_logger).HandleAddContactResponse(xml);
            }           
        }
    }
}