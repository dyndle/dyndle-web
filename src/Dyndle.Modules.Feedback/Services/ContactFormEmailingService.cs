using System;
using Dyndle.Modules.Core.Extensions;
using Dyndle.Modules.Email.Services;
using Dyndle.Modules.Feedback.Contracts;
using Dyndle.Modules.Feedback.Models;

namespace Dyndle.Modules.Feedback.Services
{
    /// <summary>
    /// Used to send contact form related emails
    /// </summary>
    public class ContactFormEmailingService : IContactFormEmailingService
    {
        private readonly IEmailRenderingService _renderingService;
        private readonly IEmailSendingService _sendingService;
        private readonly IContactConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="IContactFormEmailingService"/> class.
        /// </summary>
        /// <param name="renderingService">The rendering service.</param>
        /// <param name="sendingService">The sending service.</param>
        /// <param name="configuration">The configuration.</param>
        public ContactFormEmailingService(IEmailRenderingService renderingService, 
            IEmailSendingService sendingService, 
            IContactConfiguration configuration)
        {
            renderingService.ThrowIfNull(nameof(renderingService));
            sendingService.ThrowIfNull(nameof(sendingService));
            configuration.ThrowIfNull(nameof(configuration));

            _renderingService = renderingService;
            _sendingService = sendingService;
            _configuration = configuration;
        }

        public void SendFormData(ContactFormData formData)
        {
            // Render email body
            var htmlBody = _renderingService.RenderEmailBody(
                                    itemId: _configuration.EmailComponentId,
                                    routeValues: new
                                    {
                                        Contactform = new
                                        {
                                            Name = formData.Name,
                                            Email = formData.Email,
                                            Subject = formData.Subject,
                                            Message = formData.Message
                                        }
                                    }
                                );

            // Create email
            var message = _sendingService.CreateMessage(
                                fromAddress: _configuration.FromAddress,
                                fromName: _configuration.FromName,
                                toAddress: _configuration.ToAddress,
                                toName: _configuration.ToAddress,
                                subject: _configuration.Subject,
                                htmlBody: htmlBody
                            );

            // Send email
            _sendingService.SendMessage(message);
        }
    }
}