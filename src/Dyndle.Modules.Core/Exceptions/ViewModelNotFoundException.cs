using System;
using System.Runtime.Serialization;
using DD4T.ContentModel;

namespace Dyndle.Modules.Core.Exceptions
{
    /// <inheritdoc />
    public class ViewModelNotFoundException : Exception
    {
        /// <inheritdoc />
        /// <summary>
        /// ViewModelNotFoundException
        /// </summary>
        /// <param name="data"></param>
        public ViewModelNotFoundException(IComponentPresentation data)
            : base(string.Format("Could not find view model for schema '{0}' and Template '{1}' or default for schema '{0}' in loaded assemblies."
                , data.Component.Schema.Title, data.ComponentTemplate.Title)) 
        { }

        /// <inheritdoc />
        /// <summary>
        /// ViewModelNotFoundException
        /// </summary>
        /// <param name="data"></param>
        public ViewModelNotFoundException(ITemplate data)
            : base($"Could not find view model for item with Template '{data.Title}' and ID '{data.Id}'")
        { }

        /// <inheritdoc />
        /// <summary>
        /// ViewModelNotFoundException that takes SerializationInfo and StreamingContext
        /// </summary>
        /// <param name="info">The SerializationInfo.</param>
        /// <param name="context">The StreamingContext.</param>
        public ViewModelNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        //TODO: REfactor to check type and use other overloads
        /// <inheritdoc />
        /// <summary>
        /// ViewModelNotFoundException
        /// </summary>
        /// <param name="data"></param>
        public ViewModelNotFoundException(IModel data)
            : base(GetErrorMessageForModel(data))
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

        private static string GetErrorMessageForModel(IModel model)
        {
            if (model is IPage)
            {
                IPage page = (IPage)model;
                return $"Could not find view model for page {page.Title} ({page.Id}) with page template {page.PageTemplate.Title} ({page.PageTemplate.Id})";
            }
            if (model is IComponentPresentation)
            {
                IComponentPresentation cp = (IComponentPresentation)model;
                return $"Could not find view model for component with {cp.Component.Title} ({cp.Component.Id}) with schema {cp.Component.Schema.Title} ({cp.Component.Schema.Id})";
            }
            return $"Could not find view model for item {model}";
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
