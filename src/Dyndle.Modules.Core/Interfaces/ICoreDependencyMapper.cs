using System;
using System.Collections.Generic;

namespace Dyndle.Modules.Core.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICoreDependencyMapper
    {
        IDictionary<Type, Type> ActionFilterMappings { get; }

        IDictionary<Type, Type> DevEnabledPerLifeTimeMappings { get; }

        IDictionary<Type, Type> ProdEnabledPerLifeTimeMappings { get; }

        IDictionary<Type, Type> EnabledPerLifeTimeMappings { get; }

        IDictionary<Type, Type> DisabledPerLifeTimeMappings { get; }
    }
}
