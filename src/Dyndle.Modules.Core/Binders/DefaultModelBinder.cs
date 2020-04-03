using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;

namespace Dyndle.Modules.Core.Binders
{
    public class DefaultModelBinder : System.Web.Mvc.DefaultModelBinder
    {
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

        PropertyBinderAttribute TryFindPropertyBinderAttribute(PropertyDescriptor propertyDescriptor)
        {
            return propertyDescriptor.Attributes
              .OfType<PropertyBinderAttribute>()
              .FirstOrDefault();
        }
    }
}
