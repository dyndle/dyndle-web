using System;
using System.Collections.Generic;
using System.Web.Mvc;
using DD4T.ContentModel;
using DD4T.Core.Contracts.Resolvers;
using DD4T.ViewModels.Attributes;
using DD4T.ViewModels.Base;
using Dyndle.Modules.Core.Attributes.ViewModels;
using Dyndle.Modules.Core.Models.System;
using Newtonsoft.Json;

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
        [Multimedia]
        public IMultimedia Multimedia { get; set; }

    }
}