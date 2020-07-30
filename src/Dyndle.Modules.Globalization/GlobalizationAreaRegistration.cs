using Dyndle.Modules.Core.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Dyndle.Modules.Globalization
{
    public class GlobalizationAreaRegistration : BaseModuleAreaRegistration
    {
        public override string AreaName => "Globalization";

        public override void RegisterTypes(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton(typeof(IGlobalizationService), typeof(GlobalizationService));
        }
    }
}