using Microsoft.Extensions.DependencyInjection;
using Trivident.Modules.Core.Configuration;
using Trivident.Modules.Email.Services;

namespace Trivident.Modules.Email
{
	/// <inheritdoc />
	public class EmailAreaRegistration : BaseModuleAreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Email";
            }
        }

	    /// <inheritdoc />
	    public override void RegisterTypes(IServiceCollection serviceCollection)
	    {
			serviceCollection.AddSingleton(typeof(IEmailRenderingService), typeof(DefaultEmailRenderingService));
		    serviceCollection.AddSingleton(typeof(IEmailSendingService), typeof(DefaultEmailSendingService));
		}

	}
}