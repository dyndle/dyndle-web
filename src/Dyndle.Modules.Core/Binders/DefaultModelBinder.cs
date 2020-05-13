using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;

namespace Dyndle.Modules.Core.Binders
{
    /// <summary>
    /// Class DefaultModelBinder.
    /// Implements the <see cref="System.Web.Mvc.DefaultModelBinder" />
    /// </summary>
    /// <seealso cref="System.Web.Mvc.DefaultModelBinder" />
    public class DefaultModelBinder : System.Web.Mvc.DefaultModelBinder
    {
        /// <summary>Binds the specified property by using the specified controller context and binding context and the specified property descriptor.</summary>
        /// <param name="controllerContext">The context within which the controller operates. The context information includes the controller, HTTP content, request context, and route data.</param>
        /// <param name="bindingContext">
        /// The context within which the model is bound. The context includes information such as the model object, model name, model type, property filter, and value provider.
        /// </param>
        /// <param name="propertyDescriptor">
        /// Describes a property to be bound. The descriptor provides information such as the component type, property type, and property value. It also provides methods to get or set the property value.
        /// </param>
        protected override void BindProperty(
            ControllerContext controllerContext,
            ModelBindingContext bindingContext,
            PropertyDescriptor propertyDescriptor)
        {
            var propertyBinderAttribute = TryFindPropertyBinderAttribute(propertyDescriptor);
            if (propertyBinderAttribute != null)
            {
                var subPropertyName = CreateSubPropertyName(bindingContext.ModelName, propertyDescriptor.Name);

                var context = new ModelBindingContext()
                {
                    ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(null, propertyDescriptor.PropertyType),
                    ModelName = subPropertyName,
                    ModelState = bindingContext.ModelState,
                    PropertyFilter = bindingContext.PropertyFilter,
                    ValueProvider = bindingContext.ValueProvider,
                    FallbackToEmptyPrefix = true
                };

                var binder = propertyBinderAttribute.CreateBinder();
                var value = GetPropertyValue(controllerContext, context, propertyDescriptor, binder);

                if (value != null)
                {
                    propertyDescriptor.SetValue(bindingContext.Model, value);
                }
            }
            else // revert to the default behavior.
            {
                base.BindProperty(controllerContext, bindingContext, propertyDescriptor);
            }
        }

        private PropertyBinderAttribute TryFindPropertyBinderAttribute(PropertyDescriptor propertyDescriptor)
        {
            return propertyDescriptor.Attributes
              .OfType<PropertyBinderAttribute>()
              .FirstOrDefault();
        }
    }
}
