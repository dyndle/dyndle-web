using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Trivident.Modules.Core.Binders
{
    public class DefaultModelBinder : System.Web.Mvc.DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            return base.BindModel(controllerContext, bindingContext);
        }
        protected override object GetPropertyValue(ControllerContext controllerContext, ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor, IModelBinder propertyBinder)
        {
            return base.GetPropertyValue(controllerContext, bindingContext, propertyDescriptor, propertyBinder);
        }
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
