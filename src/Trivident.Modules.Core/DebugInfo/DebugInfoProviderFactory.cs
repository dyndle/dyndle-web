using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Trivident.Modules.Core.DebugInfo
{
    public static class DebugInfoProviderFactory
    {
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