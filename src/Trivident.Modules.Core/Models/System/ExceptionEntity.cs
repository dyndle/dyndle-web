using DD4T.ContentModel;
using Trivident.Modules.Core.Attributes.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DD4T.Core.Contracts.ViewModels;

namespace Trivident.Modules.Core.Models.System
{
    /// <summary>
    /// Class ExceptionEntity.
    /// Used to replace models whenever an exception occurs during the mapping of the data
    /// This prevents the page from failing entirely
    /// </summary>
    /// <seealso cref="Trivident.Modules.Core.Models.EntityModel" />
    public class ExceptionEntity : EntityModel
    {
        /// <summary>
        /// Gets the exception.
        /// </summary>
        /// <value>The exception.</value>
        public Exception Exception { get; private set; }

        /// <summary>
        /// Gets the original MVC data.
        /// </summary>
        /// <value>The original MVC data.</value>
        public MvcData OriginalMvcData { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionEntity"/> class.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="model">The model.</param>
        public ExceptionEntity(Exception exception, IModel model)
        {
            Exception = exception;
            this.OriginalMvcData = CustomRenderDataAttribute.GetMvcData(model);
            var mvcData = OriginalMvcData ?? new MvcData();

            mvcData.Area = "Core";
            mvcData.View = "EntityError";
            mvcData.Controller = "Entity";
            mvcData.Action = "Entity";

            ((IViewModel)this).ModelData = model;

            this.MvcData = mvcData;
        }
    }
}