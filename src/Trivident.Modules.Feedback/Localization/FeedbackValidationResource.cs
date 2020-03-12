using Trivident.Modules.Core.Environment;
using System.Web.Mvc;

namespace Trivident.Modules.Feedback.Localization
{
    public class FeedbackValidationResource
    {
        public static string TitleRequired { get { return GetLabel(FeedbackConstants.Labels.TitleRequired); } }
        public static string NameRequired { get { return GetLabel(FeedbackConstants.Labels.NameRequired); } }
        public static string FirstNameRequired { get { return GetLabel(FeedbackConstants.Labels.FirstNameRequired); } }
        public static string AddressRequired { get { return GetLabel(FeedbackConstants.Labels.AddressRequired); } }
        public static string PostalCodeRequired { get { return GetLabel(FeedbackConstants.Labels.PostalCodeRequired); } }
        public static string CityRequired { get { return GetLabel(FeedbackConstants.Labels.CityRequired); } }
        public static string CountryRequired { get { return GetLabel(FeedbackConstants.Labels.CountryRequired); } }
        public static string EmailRequired { get { return GetLabel(FeedbackConstants.Labels.EmailRequired); } }
        public static string ConfirmEmailRequired { get { return GetLabel(FeedbackConstants.Labels.ConfirmEmailRequired); } }
        public static string AffectedProductRequired { get { return GetLabel(FeedbackConstants.Labels.AffectedProductRequired); } }
        public static string ReasonRequired { get { return GetLabel(FeedbackConstants.Labels.ReasonRequired); } }
        public static string MessageRequired { get { return GetLabel(FeedbackConstants.Labels.MessageRequired); } }
        public static string AcceptTermsConditionsRequired { get { return GetLabel(FeedbackConstants.Labels.AcceptTermsConditionsRequired); } }
        public static string EmailsDoNotMatch { get { return GetLabel(FeedbackConstants.Labels.EmailsDoNotMatch); } }
        public static string IncorrectEmailAddress { get { return GetLabel(FeedbackConstants.Labels.IncorrectEmailAddress); } }
        
        public static string GetLabel(string name)
        {
            return DependencyResolver.Current.GetService<ISiteContext>().GetLabel("ContactForm." + name);
        }
    }
}