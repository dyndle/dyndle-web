using System;
using System.Collections.Generic;
using System.Linq;

namespace Dyndle.Modules.Core.DebugInfo
{
    /// <summary>
    /// Class DebugInfoProviderFactory.
    /// </summary>
    public static class DebugInfoProviderFactory
    {
        /// <summary>
        /// Gets the providers.
        /// </summary>
        /// <value>The providers.</value>
        public static IEnumerable<IDebugInfoProvider> Providers
        {
            get
            {
                var type = typeof(IDebugInfoProvider);
                var types = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(s => s.GetTypes())
                    .Where(p => type.IsAssignableFrom(p) && p.IsClass && !p.IsAbstract);
                return types.Select(t => Activator.CreateInstance(t) as IDebugInfoProvider);
            }
        }
    }
}