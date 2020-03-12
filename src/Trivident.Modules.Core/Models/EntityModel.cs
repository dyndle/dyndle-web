using DD4T.ViewModels.Attributes;
using DD4T.ViewModels.Base;
using Trivident.Modules.Core.Attributes.ViewModels;
using Trivident.Modules.Core.Models.System;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Web.Mvc;
using DD4T.ContentModel.Factories;
using DD4T.Core.Contracts.Resolvers;
using Newtonsoft.Json;

namespace Trivident.Modules.Core.Models
{
    /// <summary>
    /// Class EntityModel.
    /// Used as base class for all DD4T component models
    /// </summary>
    /// <seealso cref="DD4T.ViewModels.Base.ViewModelBase" />
    /// <seealso cref="Trivident.Modules.Core.Models.IEntityModel" />
    public abstract class EntityModel : ViewModelBase, IEntityModel, IFilterable
    {
        #region IRenderable

        /// <summary>
        /// Gets or sets the MVC data.
        /// </summary>
        /// <value>The MVC data.</value>
        [CustomRenderData]
        public IMvcData MvcData { get; set; }

        /// <summary>
        /// Gets the view.
        /// </summary>
        /// <returns>System.String.</returns>
        public string GetView()
        {
            return this.MvcData?.View;
        }

        #endregion IRenderable

        #region IFilterable

        /// <summary>
        /// Filters are a generic concept to enable the content on the page to be filtered based 
        /// on implementation specific logic (for example TargetGroups or component metadata)
        /// </summary>
        [Filters]
        public List<IFilter> Filters { get; set; }

        #endregion
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [ComponentTcmUri]
        public TcmUri Id { get; set; }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>The URL.</value>
        [JsonIgnore]
        public string Url
        {
            get
            {
                if (Id != null)
                {
                    return DependencyResolver.Current.GetService<ILinkResolver>().ResolveUrl(Id.ToString());
                }
                return _url;
            }
        }
        private string _url = null;

        #region Overrides

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string containing the type and identifier of the Entity.</returns>
        public override string ToString()
        {
            return (Id == null) ? GetType().Name : String.Format("{0}: {1}", GetType().Name, Id);
        }
        #endregion Overrides
    }
}