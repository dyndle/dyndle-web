using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Trivident.Modules.Feedback.Localization;
using Trivident.Modules.Feedback.Models;

namespace Trivident.Modules.Feedback.ViewModels
{
    /// <summary>
    /// View model used for Coheris feedback form
    /// </summary>
    public class FeedbackFormViewModel
    {
        [Required(ErrorMessageResourceType = typeof(FeedbackValidationResource), ErrorMessageResourceName = FeedbackConstants.Labels.TitleRequired)]
        public int Title { get; set; }

        [Required(ErrorMessageResourceType = typeof(FeedbackValidationResource), ErrorMessageResourceName = FeedbackConstants.Labels.NameRequired)]
        public string Name { get; set; }

        [Required(ErrorMessageResourceType = typeof(FeedbackValidationResource), ErrorMessageResourceName = FeedbackConstants.Labels.FirstNameRequired)]
        public string FirstName { get; set; }

        [Required(ErrorMessageResourceType = typeof(FeedbackValidationResource), ErrorMessageResourceName = FeedbackConstants.Labels.AddressRequired)]
        public string Address { get; set; }

        [Required(ErrorMessageResourceType = typeof(FeedbackValidationResource), ErrorMessageResourceName = FeedbackConstants.Labels.PostalCodeRequired)]
        public string PostalCode { get; set; }

        [Required(ErrorMessageResourceType = typeof(FeedbackValidationResource), ErrorMessageResourceName = FeedbackConstants.Labels.CityRequired)]
        public string City { get; set; }

        public string Phone { get; set; }

        [Required(ErrorMessageResourceType = typeof(FeedbackValidationResource), ErrorMessageResourceName = FeedbackConstants.Labels.CountryRequired)]
        public string Country { get; set; }

        [Required(ErrorMessageResourceType = typeof(FeedbackValidationResource), ErrorMessageResourceName = FeedbackConstants.Labels.EmailRequired)]
        [EmailAddress(ErrorMessageResourceType = typeof(FeedbackValidationResource), ErrorMessageResourceName = FeedbackConstants.Labels.IncorrectEmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(FeedbackValidationResource), ErrorMessageResourceName = FeedbackConstants.Labels.ConfirmEmailRequired)]
        [EmailAddress(ErrorMessageResourceType = typeof(FeedbackValidationResource), ErrorMessageResourceName = FeedbackConstants.Labels.IncorrectEmailAddress)]
        [System.ComponentModel.DataAnnotations.Compare("Email", ErrorMessageResourceType = typeof(FeedbackValidationResource), ErrorMessageResourceName = FeedbackConstants.Labels.EmailsDoNotMatch)]
        public string ConfirmEmail { get; set; }

        public int? AgeRange { get; set; }

        [Required(ErrorMessageResourceType = typeof(FeedbackValidationResource), ErrorMessageResourceName = FeedbackConstants.Labels.AffectedProductRequired)]
        public int AffectedProduct { get; set; }

        public string ControlNumber { get; set; }

        public long? BarCode { get; set; }

        [Required(ErrorMessageResourceType = typeof(FeedbackValidationResource), ErrorMessageResourceName = FeedbackConstants.Labels.ReasonRequired)]
        public int Reason { get; set; }

        [Required(ErrorMessageResourceType = typeof(FeedbackValidationResource), ErrorMessageResourceName = FeedbackConstants.Labels.MessageRequired)]
        public string Message { get; set; }

        [Range(typeof(bool), "true", "true", ErrorMessageResourceType = typeof(FeedbackValidationResource), ErrorMessageResourceName = FeedbackConstants.Labels.AcceptTermsConditionsRequired)]
        public bool AcceptTermsConditions { get; set; }

        public bool NewsletterOptIn { get; set; }

        // the options in the various select lists
        public IList<KeyValuePair<string, string>> Titles { get; set; }
        public IList<KeyValuePair<string, string>> Countries { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public IList<KeyValuePair<string, string>> AgeRanges { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public IList<KeyValuePair<string, string>> Products { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public IList<KeyValuePair<string, string>> Reasons { get; set; }

        // ideally use some mapper mechanism for this?
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public FeedbackFormData GetData()
        {
            return new FeedbackFormData()
            {
                Title = this.Title,
                Name = this.Name,
                FirstName = this.FirstName,
                Address = this.Address,
                PostalCode = this.PostalCode,
                City = this.City,
                Phone = this.Phone,
                Country = this.Country,
                Email = this.Email,
                AgeRange = this.AgeRange,
                AffectedProduct = this.AffectedProduct,
                ControlNumber = this.ControlNumber,
                BarCode = this.BarCode,
                Reason = this.Reason,
                Message = this.Message,
                NewsletterOptIn = this.NewsletterOptIn
            };
        }
    }
}