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
            if (data is IComponentPresentation)
            {
                Identifier = $"ComponentPresentation component.title={((IComponentPresentation)data).Component.Title}, component.id={((IComponentPresentation)data).Component.Id}";
            }
            else if (data is IPage)
            {
                Identifier = $"Page title={((IPage)data).Title}, id={((IPage)data).Id}, page template title={((IPage)data).PageTemplate.Title}";
            }

            else if (data is ITemplate)
            {
                Identifier = $"template title={((ITemplate)data).Title}, id={((ITemplate)data).Id}";
            }
        }

        private static string GetErrorMessageForModel(IModel model, Type modelType)
        {
            if (model is IPage)
            {
                IPage page = (IPage)model;
                return $"Could not build view model of type {modelType.FullName} from page {page.Title} ({page.Id}) with page template {page.PageTemplate.Title} ({page.PageTemplate.Id})";
            }
            if (model is IComponentPresentation)
            {
                IComponentPresentation cp = (IComponentPresentation)model;
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
