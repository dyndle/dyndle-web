using System.Net.Mail;

namespace Dyndle.Modules.Email.Services
{
    public interface IEmailSendingService
    {
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
        MailMessage CreateMessage(string fromAddress, string fromName, string toAddress, string toName, string subject, string htmlBody);
        /// <summary>
        /// Sends the message using SMTP.
        /// </summary>
        /// <param name="message">The message to send.</param>
        void SendMessage(MailMessage message);
    }
}