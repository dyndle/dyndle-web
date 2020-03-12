using DD4T.ContentModel;
using DD4T.Core.Contracts.ViewModels;
using DD4T.ViewModels.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Trivident.Modules.Core.Attributes.ViewModels
{
    /// <summary>
    /// Binds the TcmUri to a property of your (View)Model
    /// </summary>
    /// <seealso cref="DD4T.ViewModels.Attributes.ComponentAttributeBase" />
    public class ComponentTcmUriAttribute : ComponentAttributeBase
    {
        /// <summary>
        /// Gets the property values.
        /// </summary>
        /// <param name="component">The component.</param>
        /// <param name="property">The property.</param>
        /// <param name="template">The template.</param>
        /// <param name="factory">The factory.</param>
        /// <returns>IEnumerable.</returns>
        public override IEnumerable GetPropertyValues(IComponent component, IModelProperty property, ITemplate template, IViewModelFactory factory)
        {
            IEnumerable result = null;
            result = new TcmUri[] { new TcmUri(component.Id) };
            return result;
        }

        /// <summary>
        /// Gets the expected type of the return.
        /// </summary>
        /// <value>The expected type of the return.</value>
        public override Type ExpectedReturnType
        {
            get { return typeof(TcmUri); }
        }
    }
}