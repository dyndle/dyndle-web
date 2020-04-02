using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using DD4T.ContentModel.Contracts.Logging;
using DD4T.Core.Contracts.ViewModels;
using Dyndle.Modules.Core.Controllers;
using Dyndle.Modules.Core.Providers.Filter;
using Microsoft.Extensions.DependencyInjection;

namespace Dyndle.Modules.Core
{
    /// <summary>
    /// Class CoreApplicationBase.
    /// Base class for Applications build with this module.
    /// Responsible for the initialization of the application
    /// </summary>
    /// <seealso cref="System.Web.HttpApplication" />
    public abstract class CoreApplicationBase : HttpApplication
    {
        private static bool _initialized;

	    /// <summary>
	    /// Get Dependency Resolver
	    /// </summary>
	    /// <param name="serviceCollection"></param>
	    /// <returns></returns>
	    protected abstract IDependencyResolver GetDependencyResolver(IServiceCollection serviceCollection);

        /// <summary>
        /// First method triggered during startup
        /// Initializes routing, global filers and other global configurations
        /// To bootstrap dependency injection, override the OnApplicationStarting() method
        /// in your own Web / Mvc project
        /// </summary>
        protected void Application_Start()
        {
            OnApplicationStarting();
            Bootstrap.Run();
			DependencyResolver.SetResolver(this.GetDependencyResolver(Bootstrap.ServiceCollection));
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            FilterConfig.RegisterFilterProviders(FilterProviders.Providers);

            ModelBinders.Binders.DefaultBinder = new Binders.DefaultModelBinder();

            LoadViewModels();

            OnApplicationStarted();
            _initialized = true;
        }

        /// <summary>
        /// Called when [application starting]. Can be used to add additional startup logic
        /// </summary>
        protected virtual void OnApplicationStarting()
        {
        }

        /// <summary>
        /// Called when [application started]. Can be used to add additional startup logic
        /// </summary>
        protected virtual void OnApplicationStarted()
        {
        }

        /// <summary>
        /// Loads the DD4T view models from loaded assemblies.
        /// </summary>
        private void LoadViewModels()
        {
            var viewModelFactory = DependencyResolver.Current.GetService<IViewModelFactory>();
            viewModelFactory?.LoadViewModels(Bootstrap.ViewModelAssemblies);
        }

        /// <summary>
        /// Handles the Error event of the Application.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Application_Error(object sender, EventArgs e)
        {
            var logger = DependencyResolver.Current.GetService<ILogger>();
            var exception = Server.GetLastError();

            logger.Error(exception.ToString(), LoggingCategory.System);

            if (!Context.IsCustomErrorEnabled || !_initialized ||
                DebugInfoFilterProvider.IsDebugMode(new HttpContextWrapper(Context))) return;

            ShowCustomErrorPage();
            Server.ClearError();
        }

        /// <summary>
        /// Shows a custom error page by invoking the <see cref="PageController"/>.
        /// </summary>
        /// <param name="exception">The exception.</param>
        private void ShowCustomErrorPage()
        {
            var routeData = new RouteData();

            routeData.Values.Add("controller", "Page");
            routeData.Values.Add("area", "Core");
            routeData.Values.Add("action", "HttpException");

            IController controller = DependencyResolver.Current.GetService<PageController>();

            controller.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
        }
    }
}