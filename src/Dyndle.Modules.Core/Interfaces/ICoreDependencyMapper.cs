using System;
using System.Collections.Generic;

namespace Dyndle.Modules.Core.Interfaces
{
    /// <summary>
    /// Interface ICoreDependencyMapper
    /// </summary>
    public interface ICoreDependencyMapper
    {
        /// <summary>
        /// Gets the action filter mappings.
        /// </summary>
        /// <value>The action filter mappings.</value>
        IDictionary<Type, Type> ActionFilterMappings { get; }

        /// <summary>
        /// Gets the dev enabled per life time mappings.
        /// </summary>
        /// <value>The dev enabled per life time mappings.</value>
        IDictionary<Type, Type> DevEnabledPerLifeTimeMappings { get; }

        /// <summary>
        /// Gets the product enabled per life time mappings.
        /// </summary>
        /// <value>The product enabled per life time mappings.</value>
        IDictionary<Type, Type> ProdEnabledPerLifeTimeMappings { get; }

        /// <summary>
        /// Gets the enabled per life time mappings.
        /// </summary>
        /// <value>The enabled per life time mappings.</value>
        IDictionary<Type, Type> EnabledPerLifeTimeMappings { get; }

        /// <summary>
        /// Gets the disabled per life time mappings.
        /// </summary>
        /// <value>The disabled per life time mappings.</value>
        IDictionary<Type, Type> DisabledPerLifeTimeMappings { get; }
    }
}
