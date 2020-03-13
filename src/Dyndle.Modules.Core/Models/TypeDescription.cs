using System;
using System.Collections.Generic;

namespace Dyndle.Modules.Core.Models
{
    /// <summary>
    /// Class TypeDescriptionList.
    /// Defines a collection containing instructions to inject dependencies in the application
    /// </summary>
    /// <seealso />
    public class TypeDescriptionList : List<TypeDescription>
    {
        /// <summary>
        /// Adds the specified implementation type.
        /// </summary>
        /// <param name="implementationType">Type of the implementation.</param>
        /// <param name="interfaceType">Type of the interface.</param>
        /// <param name="lifeCycle">The life cycle.</param>
        public void Add(Type implementationType, Type interfaceType, LifeCycle lifeCycle = LifeCycle.SingleInstance)
        {
            this.Add(new TypeDescription()
            {
                Interface = interfaceType,
                Implementation = implementationType,
                LifeCycle = lifeCycle
            });
        }
    }

    /// <summary>
    /// Struct TypeDescription
    /// Structure that holds information needed for dependency injection
    /// </summary>
    public struct TypeDescription
    {
        /// <summary>
        /// Gets or sets the interface.
        /// </summary>
        /// <value>The interface.</value>
        public Type Interface { get; set; }
        /// <summary>
        /// Gets or sets the implementation.
        /// </summary>
        /// <value>The implementation.</value>
        public Type Implementation { get; set; }
        /// <summary>
        /// Gets or sets the life cycle.
        /// </summary>
        /// <value>The life cycle.</value>
        public LifeCycle LifeCycle { get; set; }
    }

    /// <summary>
    /// Enum LifeCycle
    /// Defines the Lifetime for the registered implementation
    /// </summary>
    public enum LifeCycle
    {
        /// <summary>
        /// The per request
        /// </summary>
        PerRequest,
        /// <summary>
        /// The single instance
        /// </summary>
        SingleInstance,
        /// <summary>
        /// The per life time
        /// </summary>
        PerLifeTime
    }
}