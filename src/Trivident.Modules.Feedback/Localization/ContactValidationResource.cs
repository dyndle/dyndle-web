using Trivident.Modules.Core.Environment;
using System.Web.Mvc;

namespace Trivident.Modules.Feedback.Localization
{
    public class ContactValidationResource
    {
        public static string NameRequired { get { return GetLabel(FeedbackConstants.Labels.NameRequired); } }
        public static string EmailRequired { get { return GetLabel(FeedbackConstants.Labels.EmailRequired); } }
        public static string SubjectRequired { get { return GetLabel(FeedbackConstants.Labels.SubjectRequired); } }
        public static string MessageRequired { get { return GetLabel(FeedbackConstants.Labels.MessageRequired); } }
        public static string AcceptTermsConditionsRequired { get { return GetLabel(FeedbackConstants.Labels.AcceptTermsConditionsRequired); } }
        public static string IncorrectEmailAddress { get { return GetLabel(FeedbackConstants.Labels.IncorrectEmailAddress); } }
        
        public static string GetLabel(string name)
        {
            return DependencyResolver.Current.GetService<ISiteContext>().GetLabel("ContactForm." + name);
        }
    }
}