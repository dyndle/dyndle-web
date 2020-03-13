using System;
using System.IO;
using System.Xml.Serialization;
using DD4T.ContentModel.Contracts.Logging;
using Dyndle.Modules.Core.Extensions;
using Dyndle.Modules.Feedback.Models;

namespace Dyndle.Modules.Feedback.Handlers
{
    /// <summary>
    /// Handles service response conversion
    /// </summary>
    public class CoherisResponseHandler
    {
        private readonly ILogger _logger;

        /// <summary>
        /// Injected dependencies
        /// </summary>
        /// <param name="logger"></param>
        public CoherisResponseHandler(ILogger logger)
        {
            logger.ThrowIfNull(nameof(logger));
            _logger = logger;
        }

        /// <summary>
        /// Converts a xml response to a SendResult
        /// </summary>
        /// <param name="xml">XML string</param>
        /// <returns>A SendResult object</returns>
        public SendResult HandleAddContactResponse(string xml)
        {
            if (string.IsNullOrEmpty(xml))
            {
                return new SendResult();
            }
            // first try to deserialize as a registration response
            var result = TryToDeserializeToType<RegistrationResponse, AddContactResponse>(xml);
            if (result == null)
            {
                // try to deserialize as a demanderesponse
                result = TryToDeserializeToType<DemandeResponse, AddContactResponse>(xml) ?? new AddContactResponse();
            }
            return new SendResult
            {
                Success = result.Success,
                Message = result.Message
            };
        }

        /// <summary>
        /// Tries to deserialize the given xml string to the specified type
        /// </summary>
        /// <typeparam name="TDeserialize"></typeparam>
        /// <typeparam name="TOutput"></typeparam>
        /// <param name="xml"></param>
        /// <returns></returns>
        private TOutput TryToDeserializeToType<TDeserialize, TOutput>(string xml) 
            where TDeserialize: class, TOutput
            where TOutput: class
        {
            TOutput result = null;
            try
            {
                var serializer = new XmlSerializer(typeof(TDeserialize));
                // make sure string reader is disposed of
                using (StringReader sr = new StringReader(xml))
                {
                    result = serializer.Deserialize(sr) as TDeserialize;
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Error deserializing Coheris response: {ex.Message}");
            }
            return result;
        }
    }
}