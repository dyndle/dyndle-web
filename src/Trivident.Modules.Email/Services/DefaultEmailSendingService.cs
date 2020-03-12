using HtmlAgilityPack;
using Trivident.Modules.Core.Contracts;
using System;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;

namespace Trivident.Modules.Email.Services
{
    /// <summary>
    /// Class DefaultEmailSendingService.
    /// Used to send email using SMTP
    /// </summary>
    /// <seealso cref="Trivident.Modules.Email.Services.IEmailSendingService" />
    public class DefaultEmailSendingService : IEmailSendingService
    {
        private readonly SmtpClient _smtpClient;
        private readonly IExtendedPublicationResolver _publicationResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultEmailSendingService"/> class.
        /// </summary>
        public DefaultEmailSendingService(IExtendedPublicationResolver publicationResolver)
        {
            publicationResolver.ThrowIfNull(nameof(publicationResolver));

            _publicationResolver = publicationResolver;
            _smtpClient = new SmtpClient();
        }

        /// <summary>
        /// Creates the message and links all resources in there.
        /// </summary>
        /// <param name="fromAddress">From address.</param>
        /// <param name="fromName">From name.</param>
        /// <param name="toAddress">To address.</param>
        /// <param name="toName">To name.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="htmlBody">The HTML body.</param>
        /// <returns>Created MailMessage</returns>
        public MailMessage CreateMessage(string fromAddress, string fromName, string toAddress, string toName, string subject, string htmlBody)
        {
            var from = new MailAddress(fromAddress, fromName);
            var to = new MailAddress(toAddress, toName);

            var message = new MailMessage(from, to);

            message.Subject = subject;

            SetBodyAndLinkResources(message, htmlBody);

            message.IsBodyHtml = true;

            return message;
        }

        /// <summary>
        /// Sets the body and links any resources.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="htmlBody">The HTML body.</param>
        private void SetBodyAndLinkResources(MailMessage message, string htmlBody)
        {
            var document = new HtmlDocument();
            document.LoadHtml(htmlBody);

            var baseUri = _publicationResolver.GetBaseUri();

            LinkResources(document, string.Format(".//*[@{0}]", "src"), "src", baseUri);
            LinkResources(document, string.Format(".//*[@{0}]", "href"), "href", baseUri);

            AlternateView alternateView = AlternateView.CreateAlternateViewFromString(document.DocumentNode.OuterHtml, Encoding.UTF8, MediaTypeNames.Text.Html);

            message.AlternateViews.Add(alternateView);
        }


        /// <summary>
        /// Links the resources from the html as embedded resources in the email
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="nodeSelector">The node selector to use when searching for resources.</param>
        /// <param name="attributeName">Name of the attribute to use as source.</param>
        /// <param name="baseUri">The base URI.</param>
        private void LinkResources(HtmlDocument document, string nodeSelector, string attributeName, Uri baseUri)
        {
            var nodes = document.DocumentNode.SelectNodes(nodeSelector);
            if (nodes == null)
                return;

            foreach (var node in nodes)
            {
                var url = node.GetAttributeValue(attributeName, null);
                if (!string.IsNullOrWhiteSpace(url) && url.StartsWith("/"))
                {
                    url = new Uri(baseUri, url).ToString();
                    node.SetAttributeValue(attributeName, url);
                }
            }
        }

        /// <summary>
        /// Sends the message using SMTP.
        /// </summary>
        /// <param name="message">The message to send.</param>
        public void SendMessage(MailMessage message)
        {
            _smtpClient.Send(message);
        }
    }
}
