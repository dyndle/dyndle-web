namespace Trivident.Modules.Feedback
{
    public class FeedbackConstants
    {
        public static class Settings
        {
            // Coheris feedback form related
            public readonly static string CoherisSiteId = "Feedback.Coheris.SiteId";
            public readonly static string ThankYouUrl = "Feedback.ThankYouUrl";

            // contact form
            public readonly static string ContactFormEmailFromAddress = "ContactForm.Email.FromAddress";
            public readonly static string ContactFormEmailToAddress = "ContactForm.Email.ToAddress";
            public readonly static string ContactFormEmailFromName = "ContactForm.Email.FromName";
            public readonly static string ContactFormEmailSubject = "ContactForm.Email.Subject";
            public readonly static string ContactFormEmailThankYouUrl = "ContactForm.Email.ThankYouUrl";
            public readonly static string ContactFormEmailEmailComponentId = "ContactForm.Email.EmailComponentId";
        }

        public static class Labels
        {
            public readonly static string Address = "ContactForm.Address";
            public readonly static string AffectedProduct = "ContactForm.AffectedProduct";
            public readonly static string AgeRange = "ContactForm.AgeRange";
            public readonly static string Agreement = "ContactForm.Agreement";
            public readonly static string BarCode = "ContactForm.BarCode";
            public readonly static string BarCodeExplanation = "ContactForm.BarCodeExplanation";
            public readonly static string City = "ContactForm.City";
            public readonly static string ConfirmEmail = "ContactForm.ConfirmEmail";
            public readonly static string ControlNumber = "ContactForm.ControlNumber";
            public readonly static string ControlNumberExplanation = "ContactForm.ControlNumberExplanation";
            public readonly static string Country = "ContactForm.Country";
            public readonly static string Email = "ContactForm.Email";
            public readonly static string FirstName = "ContactForm.FirstName";
            public readonly static string Message = "ContactForm.Message";
            public readonly static string Name = "ContactForm.Name";
            public readonly static string Phone = "ContactForm.Phone";
            public readonly static string PostalCode = "ContactForm.PostalCode";
            public readonly static string Reason = "ContactForm.Reason";
            public readonly static string Subject = "ContactForm.Subject";
            public readonly static string Title = "ContactForm.Title";
            public readonly static string TermsConditions = "ContactForm.TermsConditions";
            public readonly static string SignedUpNewsletter = "ContactForm.SignedUpNewsletter";
            public readonly static string Send = "ContactForm.Send";

            // Validation messages, string value should appear as constant in FeedbackValidationResource
            public const string TitleRequired = "TitleRequired";
            public const string NameRequired = "NameRequired";
            public const string FirstNameRequired = "FirstNameRequired";
            public const string AddressRequired = "AddressRequired";
            public const string PostalCodeRequired = "PostalCodeRequired";
            public const string CityRequired = "CityRequired";
            public const string CountryRequired = "CountryRequired";
            public const string EmailRequired = "EmailRequired";
            public const string ConfirmEmailRequired = "ConfirmEmailRequired";
            public const string AffectedProductRequired = "AffectedProductRequired";
            public const string ReasonRequired = "ReasonRequired";
            public const string MessageRequired = "MessageRequired";
            public const string AcceptTermsConditionsRequired = "AcceptTermsConditionsRequired";
            public const string EmailsDoNotMatch = "EmailsDoNotMatch";
            public const string IncorrectEmailAddress = "IncorrectEmailAddress";
            public const string SubjectRequired = "SubjectRequired";

        }
    }
}