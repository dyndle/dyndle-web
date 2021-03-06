﻿using System.Web.Mvc;
using DD4T.ContentModel.Contracts.Logging;
using Dyndle.Modules.Core.Attributes.Filter;
using Dyndle.Modules.Core.Configuration;
using Dyndle.Modules.Core.Providers.Filter;

namespace Dyndle.Modules.Core
{
    /// <summary>
    /// Class FilterConfig.
    /// Used to group all filter related configurations 
    /// </summary>
    public static class FilterConfig
    {
        /// <summary>
        /// Registers the global filters.
        /// </summary>
        /// <param name="filters">The filters.</param>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //Add filters to show section errors (if enabled in the configuration)
            if (DyndleConfig.EnableSectionErrors)
            {
                var logger = DependencyResolver.Current.GetService<ILogger>();
                filters.Add(new HandleSectionErrorAttribute(logger));
            }
        }

        /// <summary>
        /// Registers the filter providers.
        /// </summary>
        /// <param name="filterProviders">The filter providers.</param>
        public static void RegisterFilterProviders(FilterProviderCollection filterProviders)
        {
            //Add filterproviders
            filterProviders.Add(new DebugInfoFilterProvider());
        }
    }
}