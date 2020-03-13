namespace Dyndle.Modules.Feedback.Contracts
{
    /// <summary>
    /// Configuration for the contact form functionality 
    /// </summary>
    public interface IContactConfiguration
    {
        string FromAddress { get; }
        string ToAddress { get; }
        string FromName { get; }
        string Subject { get; }
        string ThankYouUrl { get; }
        int EmailComponentId { get; }
    }
}
