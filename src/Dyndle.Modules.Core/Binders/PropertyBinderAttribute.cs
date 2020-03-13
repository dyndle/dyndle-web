using System;
using System.Web.Mvc;

namespace Dyndle.Modules.Core.Binders
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Attribute" />
    public class PropertyBinderAttribute : Attribute
    {
        private readonly Type _binderType;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyBinderAttribute"/> class.
        /// </summary>
        /// <param name="binderType">Type of the binder.</param>
        public PropertyBinderAttribute(Type binderType)
        {
            _binderType = binderType;
        }

        /// <summary>
        /// Creates the binder.
        /// </summary>
        /// <returns></returns>
        public virtual IModelBinder CreateBinder()
        {
            return (IModelBinder)DependencyResolver.Current.GetService(_binderType);
        }
    }
}
