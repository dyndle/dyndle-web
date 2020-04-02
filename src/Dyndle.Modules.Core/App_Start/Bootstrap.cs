using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Compilation;
using System.Web.Routing;
using Dyndle.Modules.Core.Configuration;
using Dyndle.Modules.Core.Extensions;
using Dyndle.Modules.Core.Modules;
using Microsoft.Extensions.DependencyInjection;

namespace Dyndle.Modules.Core
{
    /// <summary>
    /// DI container setup; class was created to make DI setup accessible from both MVC startup class, but also from
    /// OWIN module ie for Authentication, so that in OWIN module we can also use the same dependency resolving
    /// </summary>
    public class Bootstrap
    {
		private static readonly object _locker = new object();
		private static bool _intialized;


		/// <summary>
		/// 
		/// </summary>
		public static IServiceCollection ServiceCollection { get;  } = new ServiceCollection();



		/// <summary>
		/// List of referenced assemblies, used to find all available DD4T view models 
		/// </summary>
		public static IList<Assembly> ViewModelAssemblies
		{
			get
			{
                var viewModelNamespaces = DyndleConfig.ViewModelNamespaces;

				if (string.IsNullOrWhiteSpace(viewModelNamespaces))
					throw new Exception($"AppSettings Key/Value is missing [Key: {CoreConstants.Configuration.ViewModelNamespaces}]");

                var namespaces = viewModelNamespaces.Split(',').Select(n => n.Trim());
				return BuildManager.GetReferencedAssemblies().Cast<Assembly>()
					.Where(a => !a.GlobalAssemblyCache
					            && !a.IsDynamic
					            && !a.ReflectionOnly
					            && namespaces.Any(n => a.FullName.StartsWith(n))).ToList();
			}
		}

        /// <summary>
        /// List of referenced assemblies, used to find all MVC controllers to be registered
        /// </summary>
        public static IList<Assembly> ControllerAssemblies
        {
            get
            {
                var controllerNamespaces = DyndleConfig.ControllerNamespaces;


                if (string.IsNullOrWhiteSpace(controllerNamespaces))
                    throw new Exception($"AppSettings Key/Value is missing [Key: {CoreConstants.Configuration.ControllerNamespaces}]");

                var namespaces = controllerNamespaces.Split(',').Select(n => n.Trim());
               
                return AppDomain.CurrentDomain.GetAssemblies().Cast<Assembly>()
                    .Where(a => !a.GlobalAssemblyCache
                                && !a.IsDynamic
                                && !a.ReflectionOnly
                                && namespaces.Any(n => a.GetTypes().Any(t => t.FullName.StartsWith(n)))).ToList();
            }
        }



        /// <summary>
        /// Main method for DI container setup, will only run the container setup code once
        /// </summary>
        public static void Run()
		{
			lock (_locker)
			{
				//Check if it has already run
				if (_intialized)
				{
					return;
				}

				RegisterModules();

				//Mark as run
				_intialized = true;
			}
		}


        /// <summary>
        /// Register all necessary core/DD4T dependencies
        /// </summary>
        /// <param name="builder"></param>
        private static void RegisterModules()
        {
          
			ServiceCollection.RegisterModule<DefaultsModule>();
			ServiceCollection.RegisterModule<PreviewModule>();
			ServiceCollection.RegisterModule<RedirectionModule>();
			ServiceCollection.RegisterModule<ActionFilterModule>();

			var isDevMode = !CoreConstants.Configuration.PublicationId.GetConfigurationValue().IsNullOrEmpty();
            var isCacheEnabled = DyndleConfig.IsCacheEnabled;
            var isStaging = DyndleConfig.IsStagingSite;

			ServiceCollection.RegisterModule(new CacheModule(isCacheEnabled, isStaging));

		}

	    /// <summary>
	    /// The get executing assembly
	    /// </summary>
	    public static Assembly GetExecutingAssembly = Assembly.GetExecutingAssembly();
	}
}