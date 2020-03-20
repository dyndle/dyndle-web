using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Integration.Mvc;
using DD4T.Caching.ApacheMQ;
using DD4T.ContentModel.Contracts.Caching;
using DD4T.ContentModel.Contracts.Logging;
using DD4T.DI.Autofac;
using DD4T.Utils;
using DD4T.Utils.Caching;
using Dyndle.Modules.Core;
using Dyndle.Modules.Core.Cache;
using Dyndle.Modules.Core.Extensions;
using Dyndle.Modules.Management.Controllers;
using Microsoft.Extensions.DependencyInjection;
using RazorGenerator.Mvc;

namespace $rootnamespace$
{
    public class ApplicationBase : CoreApplicationBase
    {
        protected override IDependencyResolver GetDependencyResolver(IServiceCollection serviceCollection)
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(ApplicationBase).Assembly); // Current MVC project
            builder.RegisterControllers(typeof(CoreApplicationBase).Assembly); // Core Module
            builder.RegisterControllers(typeof(CacheController).Assembly); // Management Module
            
            builder.RegisterFilterProvider();
            builder.Populate(serviceCollection);
            // if you want to override any of the types from DD4T or Dyndle, do it below:
            //builder.RegisterType<MyViewModelFactory>().As<IViewModelFactory>().SingleInstance();

            builder.UseDD4T();
            var container = builder.Build();
            var dependencyResolver = new AutofacDependencyResolver(container);
            return dependencyResolver;
        }

        protected override void OnApplicationStarting()
        {
            // Before DI container is built and view models are loaded
        }

        protected override void OnApplicationStarted()
        {
           
            OptimizeViewEngines();
            SubscribeToJmsMessageProvider();
        }

        /// <summary>
        /// Optimizing performance by using only use the RazorEngine and PrecompiledMvcEngines - http://blogs.msdn.com/b/marcinon/archive/2011/08/16/optimizing-mvc-view-lookup-performance.aspx
        /// </summary>
        private static void OptimizeViewEngines()
        {
            var viewEngines = new List<IViewEngine>();
            foreach (var engine in ViewEngines.Engines)
            {
                if (engine is PrecompiledMvcEngine)
                {
                    viewEngines.Add(engine);
                }
            }

            ViewEngines.Engines.Clear();
            IViewEngine razorEngine = new RazorViewEngine() { FileExtensions = new string[] { "cshtml" } };
            ViewEngines.Engines.Add(razorEngine);
            foreach (var engine in viewEngines)
            {
                ViewEngines.Engines.Add(engine);
            }
        }
        private static void SubscribeToJmsMessageProvider()
        {
            if (!(string.IsNullOrEmpty(ConfigurationKeys.JMSHostname.GetConfigurationValue()) &&
                  string.IsNullOrEmpty(ConfigurationKeys.JMSUrl.GetConfigurationValue())))
            {
                if (DependencyResolver.Current.GetService<IMessageProvider>() is JMSMessageProvider messageProvider)
                {
                    var cacheAgent = DependencyResolver.Current.GetService<ICacheAgent>();
                    if (cacheAgent is DefaultCacheAgent)
                    {
                        messageProvider.Start();
                        ((DefaultCacheAgent)cacheAgent).Subscribe(messageProvider);
                    }
                    else if (cacheAgent is PreviewAwareCacheAgent)
                    {
                        messageProvider.Start();
                        // Note: the PreviewAwareCacheAgent has no Subscribe method, but if you
                        // set a MessageProvider, the Subscribe method of the wrapped DefaultCacheAgent
                        // will be called
                        ((PreviewAwareCacheAgent)cacheAgent).MessageProvider = messageProvider;
                    }
                }
            }
        }

    }
}
