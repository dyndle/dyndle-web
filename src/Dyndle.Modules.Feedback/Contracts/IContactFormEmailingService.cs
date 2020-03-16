using Dyndle.Modules.Feedback.Models;

namespace Dyndle.Modules.Feedback.Contracts
{
    /// <summary>
    /// Used to send emails for contact information
    /// </summary>
    public interface IContactFormEmailingService
    {
        /// <summary>
        /// Sends the specified result as email.
        /// </summary>
        /// <param name="formData">The result.</param>
        void SendFormData(ContactFormData formData);
    }
}
