namespace Trivident.Modules.Feedback.Models
{
    public class SendResult
    {
        public bool Success { get; set; } = false;
        public string Message { get; set; } = string.Empty;
    }
}