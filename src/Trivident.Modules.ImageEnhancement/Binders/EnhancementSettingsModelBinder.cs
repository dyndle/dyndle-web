using System;
using System.Web.Mvc;
using Dyndle.Modules.Core.Extensions;
using Trivident.Modules.ImageEnhancement.Models;

namespace Trivident.Modules.ImageEnhancement.Binders
{
    /// <summary>
    /// Class EnhancementSettingsModelBinder.
    /// Used to provide the <see cref="IModelBinderProvider"/>  models to an controller action. 
    /// Will be automatically used for all model instances implementing <see cref="IModelBinderProvider"/> when registered.
    /// </summary>
    /// <seealso cref="System.Web.Mvc.DefaultModelBinder" />
    /// <seealso cref="System.Web.Mvc.IModelBinderProvider" />
    public class EnhancementSettingsModelBinder : DefaultModelBinder, IModelBinderProvider
    {

        /// <summary>
        /// Binds the model by using the specified controller context and binding context.
        /// </summary>
        /// <param name="controllerContext">The context within which the controller operates. The context information includes the controller, HTTP content, request context, and route data.</param>
        /// <param name="bindingContext">The context within which the model is bound. The context includes information such as the model object, model name, model type, property filter, and value provider.</param>
        /// <returns>The bound object.</returns>
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            ValueProviderResult widthResult = bindingContext.ValueProvider.GetValue(ImageEnhancementConstants.ParameterNames.Width);
            ValueProviderResult heightResult = bindingContext.ValueProvider.GetValue(ImageEnhancementConstants.ParameterNames.Height);
            ValueProviderResult cropXResult = bindingContext.ValueProvider.GetValue(ImageEnhancementConstants.ParameterNames.CropX);
            ValueProviderResult cropYResult = bindingContext.ValueProvider.GetValue(ImageEnhancementConstants.ParameterNames.CropY);
            ValueProviderResult cropXPResult = bindingContext.ValueProvider.GetValue(ImageEnhancementConstants.ParameterNames.CropXP);
            ValueProviderResult cropYPResult = bindingContext.ValueProvider.GetValue(ImageEnhancementConstants.ParameterNames.CropYP);
            ValueProviderResult cropStyleResult = bindingContext.ValueProvider.GetValue(ImageEnhancementConstants.ParameterNames.CropStyle);

            IEnhancementSettings settings = new EnhancementSettings();
            if (!widthResult.IsNull())
            {
                settings.Width = Convert.ToInt32(widthResult.AttemptedValue);
            }
            if (!heightResult.IsNull())
            {
                settings.Height = Convert.ToInt32(heightResult.AttemptedValue);
            }
            if (!cropXResult.IsNull())
            {
                settings.CropX = Convert.ToInt32(cropXResult.AttemptedValue);
            }
            if (!cropYResult.IsNull())
            {
                settings.CropY = Convert.ToInt32(cropYResult.AttemptedValue);
            }
            if (!cropXPResult.IsNull())
            {
                settings.CropXP = Convert.ToInt32(cropXPResult.AttemptedValue);
            }
            if (!cropYPResult.IsNull())
            {
                settings.CropYP = Convert.ToInt32(cropYPResult.AttemptedValue);
            }
            if (!cropStyleResult.IsNull())
            {
                settings.CropStyle = (CropStyle) Enum.Parse(typeof(CropStyle), cropStyleResult.AttemptedValue);
            }

            return settings;
        }

        /// <summary>
        /// Returns the model binder for the specified type.
        /// </summary>
        /// <param name="modelType">The type of the model.</param>
        /// <returns>The model binder for the specified type.</returns>
        public IModelBinder GetBinder(Type modelType)
        {
            //Can bind to ISitemapItem
            if (typeof(IEnhancementSettings).IsAssignableFrom(modelType)) return this;
            return null;
        }
    }
}