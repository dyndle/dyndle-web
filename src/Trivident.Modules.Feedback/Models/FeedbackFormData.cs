namespace Trivident.Modules.Feedback.Models
{
    public class FeedbackFormData
    {
        public int Title { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string City  { get; set; }
        public string Phone { get; set; }
        public string Country { get; set; }
        public string Email { get; set; }
        public int? AgeRange { get; set; }
        public int AffectedProduct { get; set; }
        public string ControlNumber { get; set; }
        public long? BarCode { get; set; }
        public int Reason { get; set; }
        public string Message { get; set; }
        public bool NewsletterOptIn { get; set; }
    }
}