using Dyndle.Modules.Core.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Dyndle.Modules.Email.Services;

namespace Dyndle.Modules.Email
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