using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Dyndle.Modules.Feedback.Localization;
using Dyndle.Modules.Feedback.Models;

namespace Dyndle.Modules.Feedback.ViewModels
{
    /// <summary>
    /// View model used for contact form
    /// </summary>
    public class ContactFormViewModel
    {
        [Required(ErrorMessageResourceType = typeof(ContactValidationResource), ErrorMessageResourceName = FeedbackConstants.Labels.NameRequired)]
        public string Name { get; set; }

        [Required(ErrorMessageResourceType = typeof(ContactValidationResource), ErrorMessageResourceName = FeedbackConstants.Labels.EmailRequired)]
        [EmailAddress(ErrorMessageResourceType = typeof(ContactValidationResource), ErrorMessageResourceName = FeedbackConstants.Labels.IncorrectEmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(ContactValidationResource), ErrorMessageResourceName = FeedbackConstants.Labels.SubjectRequired)]
        public string Subject { get; set; }

        [Required(ErrorMessageResourceType = typeof(ContactValidationResource), ErrorMessageResourceName = FeedbackConstants.Labels.MessageRequired)]
        public string Message { get; set; }

        [Range(typeof(bool), "true", "true", ErrorMessageResourceType = typeof(ContactValidationResource), ErrorMessageResourceName = FeedbackConstants.Labels.AcceptTermsConditionsRequired)]
        public bool AcceptTermsConditions { get; set; }

        public bool NewsletterOptIn { get; set; }

        // the options in the various select lists
        public IList<KeyValuePair<string, string>> Subjects { get; set; }

        // ideally use some mapper mechanism for this?
        public ContactFormData GetData()
        {
            return new ContactFormData()
            {
                Name = this.Name,
                Email = this.Email,
                Subject = this.Subject,
                Message = this.Message,
                NewsletterOptIn = this.NewsletterOptIn
            };
        }
    }
}