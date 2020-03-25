﻿using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.WebPages;
using Microsoft.Extensions.DependencyInjection;
using RazorGenerator.Mvc;

namespace Dyndle.Modules.Core.Configuration
{
    /// <summary>
    /// Class BaseModuleAreaRegistration.
    /// Used as starting point to register all installed modules (and types required for those modules)
    /// </summary>
    /// <seealso cref="System.Web.Mvc.AreaRegistration" />
    public abstract class BaseModuleAreaRegistration : AreaRegistration
    {
        protected bool routesAreInitialized = false;

        /// <summary>
        /// Registers an area in an ASP.NET MVC application using the specified area's context information.
        /// Makes sure the core namespace is added so it can be used from all modules
        /// </summary>
        /// <param name="context">Encapsulates the information that is required in order to register the area.</param>
        public override void RegisterArea(AreaRegistrationContext context)
        {
            if (!routesAreInitialized)
            {
                // By default, class AreaRegistration assumes that the Controllers are in the same namespace as the concrete 
                // AreaRegistration subclass itself (or a sub namespace).
                // However, the Core controllers are in the Dyndle.Modules.Core.Controllers namespace.
                context.Namespaces.Add("Dyndle.Modules.Core.Controllers");

                context.MapRoute(
                    AreaName + "_Default",
                    Guid.NewGuid().ToString(), // Make sure the route is unique, but can never match a url
                    new { }, // No defaults. The route should never match a url and is only used for child actions
                    namespaces: context.Namespaces.ToArray());

                RegisterRoutes(context);
            }

            routesAreInitialized = true;
			RegisterTypes(Bootstrap.ServiceCollection);
        }

        /// <summary>
        /// Virtual method to allow modules to override the default routes
        /// </summary>
        /// <param name="context"></param>
        public virtual void RegisterRoutes(AreaRegistrationContext context)
        {

        }

		/// <summary>
		/// Specifies the collection of service descriptors.
		/// </summary>
		/// <param name="serviceCollection"></param>
		public virtual void RegisterTypes(IServiceCollection serviceCollection)
	    {
		    
	    }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseModuleAreaRegistration"/> class.
        /// </summary>
        protected BaseModuleAreaRegistration()
        {
            //Register embedded views for this module using a PrecompiledMvcEngine
            var engine = new PrecompiledMvcEngine(GetType().Assembly);

            //Add to the end of the list so local web-app files are used before the embedded
            ViewEngines.Engines.Add(engine);

            // StartPage lookups are done by WebPages.
            VirtualPathFactoryManager.RegisterVirtualPathFactory(engine);
        }
    }
}