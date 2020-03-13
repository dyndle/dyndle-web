using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Dyndle.Modules.Core.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Trivident.Modules.ImageEnhancement.Binders;
using Trivident.Modules.ImageEnhancement.Controllers;
using Trivident.Modules.ImageEnhancement.Services;
using Trivident.Modules.ImageEnhancement.Models;

namespace Trivident.Modules.ImageEnhancement
{
	/// <summary>
	/// Registration of Navigation Area on the core..
	/// </summary>
	public class ImageEnhancementAreaRegistration : BaseModuleAreaRegistration
	{
		/// <summary>
		/// Specify area name (this is where views will be read from)
		/// </summary>
		public override string AreaName => "ImageEnhancement";

		public override void RegisterRoutes(AreaRegistrationContext context)
		{
			context.MapRoute(
				AreaName + "_EnhancedBinaries",
				"{*url}",
				new {controller = "ImageEnhancement", action = "EnhanceImage"},

				new {page = new EnhancedImageConstraint()});

			base.RegisterRoutes(context);
		}

		public override void RegisterTypes(IServiceCollection serviceCollection)
		{
			serviceCollection.AddSingleton(typeof(IImageEnhancementController), typeof(ImageEnhancementController));
			serviceCollection.AddSingleton(typeof(IImageEnhancementService), typeof(DefaultImageEnhancementService));
			serviceCollection.AddSingleton(typeof(IModelBinderProvider), typeof(EnhancementSettingsModelBinder));
			serviceCollection.AddSingleton(typeof(IConfiguration), typeof(WebConfiguration));
		}
	}

	public class EnhancedImageConstraint : IRouteConstraint
    {
        /// <summary>
        /// Determines whether the URL parameter contains a valid value for this constraint. Returns true if URL contains any of the known enhancement attributes.
        /// </summary>
        /// <param name="httpContext">An object that encapsulates information about the HTTP request.</param>
        /// <param name="route">The object that this constraint belongs to.</param>
        /// <param name="parameterName">The name of the parameter that is being checked.</param>
        /// <param name="values">An object that contains the parameters for the URL.</param>
        /// <param name="routeDirection">An object that indicates whether the constraint check is being performed when an incoming request is being handled or when a URL is being generated.</param>
        /// <returns>true if the URL parameter contains a valid value; otherwise, false.</returns>
        public bool Match
            (
                HttpContextBase httpContext,
                Route route,
                string parameterName,
                RouteValueDictionary values,
                RouteDirection routeDirection
            )
        {
            var url = httpContext.Request.Url.ToString().ToLowerInvariant();
            return ImageEnhancementConstants.Parameters.ToLowerInvariant().Split(',').Any(x => url.Contains(x));
        }
    }
}