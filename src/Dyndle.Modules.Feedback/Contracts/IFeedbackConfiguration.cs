namespace Dyndle.Modules.Feedback.Contracts
{
    /// <summary>
    /// Send feedback form data 
    /// </summary>
    public interface IFeedbackConfiguration
    {
        int SiteId { get; }
        string ThankYouUrl { get; }
    }
}
