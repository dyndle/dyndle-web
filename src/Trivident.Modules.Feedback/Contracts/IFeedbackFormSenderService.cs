using Trivident.Modules.Feedback.Models;

namespace Trivident.Modules.Feedback.Contracts
{
    /// <summary>
    /// Send feedback form data 
    /// </summary>
    public interface IFeedbackFormSenderService
    {
        /// <summary>
        /// Send the form data to be persistend/handled
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        SendResult SendData(int siteId, FeedbackFormData data);
    }
}
