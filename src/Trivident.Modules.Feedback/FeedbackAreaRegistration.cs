using Dyndle.Modules.Core.Configuration;
using Trivident.Modules.Feedback.Configuration;
using Trivident.Modules.Feedback.Contracts;
using Trivident.Modules.Feedback.Providers;
using Trivident.Modules.Feedback.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Trivident.Modules.Feedback
{
    /// <inheritdoc />
    public class FeedbackAreaRegistration : BaseModuleAreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Feedback";
            }
        }

	    /// <inheritdoc />
	    public override void RegisterTypes(IServiceCollection serviceCollection)
	    {
			serviceCollection.AddSingleton(typeof(IFeedbackConfiguration), typeof(FeedbackConfiguration));
		    serviceCollection.AddSingleton(typeof(IFeedbackFormDataProvider), typeof(CoherisFormDataProvider));
		    serviceCollection.AddSingleton(typeof(IFeedbackFormSenderService), typeof(CoherisFeedbackFormSenderService));
		    serviceCollection.AddSingleton(typeof(IContactConfiguration), typeof(ContactConfiguration));
		    serviceCollection.AddSingleton(typeof(IContactFormEmailingService), typeof(ContactFormEmailingService));
		}

	}
}