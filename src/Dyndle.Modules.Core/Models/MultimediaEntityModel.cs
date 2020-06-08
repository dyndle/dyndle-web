using DD4T.ContentModel;
using DD4T.Core.Contracts.ViewModels;
using DD4T.ViewModels.Attributes;

namespace Dyndle.Modules.Core.Models
{
    /// <summary>
    /// Class EntityModel.
    /// Used as base class for all DD4T component models
    /// </summary>
    /// <seealso cref="DD4T.ViewModels.Base.ViewModelBase" />
    /// <seealso cref="IEntityModel" />
    public abstract class MultimediaEntityModel : EntityModel
    {
        /// <summary>
        /// Gets or sets the multimedia.
        /// </summary>
        /// <value>The multimedia.</value>
        [Multimedia]
        public IMultimedia Multimedia { get; set; }

        public IFieldSet ExternalMetadata  => GetExtensionDataField("ECL-ExternalMetadata");

        private IFieldSet GetExtensionDataField(string name)
        {
            return ((IComponentPresentation)((IViewModel)this).ModelData).Component.ExtensionData?[name];
        }

    }
}