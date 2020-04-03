using System;
using DD4T.ContentModel;

namespace Dyndle.Modules.Core.Exceptions
{
    /// <inheritdoc />
    public class ViewModelBuildException : Exception
    {

        /// <summary>
        /// ViewModelBuildException
        /// </summary>
        /// <param name="data">the generic (Tridion) object we were trying to convert to a ViewModel</param>
        public ViewModelBuildException(IModel data, Type modelType, Exception innerException)
            : base(GetErrorMessageForModel(data, modelType), innerException)
        {
            if (data is IComponentPresentation componentPresentation)
            {
                Identifier = $"ComponentPresentation component.title={componentPresentation.Component.Title}, component.id={componentPresentation.Component.Id}";
            }
            else if (data is IPage page)
            {
                Identifier = $"Page title={page.Title}, id={page.Id}, page template title={page.PageTemplate.Title}";
            }

            else if (data is ITemplate template)
            {
                Identifier = $"template title={template.Title}, id={template.Id}";
            }
        }

        private static string GetErrorMessageForModel(IModel model, Type modelType)
        {
            if (model is IPage page)
            {
                return $"Could not build view model of type {modelType.FullName} from page {page.Title} ({page.Id}) with page template {page.PageTemplate.Title} ({page.PageTemplate.Id})";
            }
            if (model is IComponentPresentation cp)
            {
                return $"Could not build view model of type {modelType.FullName} from component {cp.Component.Title} ({cp.Component.Id}) based on schema {cp.Component.Schema.Title} ({cp.Component.Schema.Id})";
            }
            return $"Could not build view model of type {modelType.FullName} from item {model}";
        }

        /// <summary>
        /// Identifier
        /// </summary>
        public string Identifier
        {
            get; set;
        }
    }
}
