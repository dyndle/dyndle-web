namespace Trivident.Modules.Feedback.Models
{
    public class ContactFormData
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public bool NewsletterOptIn { get; set; }
    }
}