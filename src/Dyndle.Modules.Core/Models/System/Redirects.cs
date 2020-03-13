using System.Collections.Generic;
using DD4T.ViewModels.Attributes;
using DD4T.ViewModels.Base;

namespace Dyndle.Modules.Core.Models.System
{
    /// <summary>
    /// Class Redirects.
    /// Used for configuring a collection of redirects in Tridion
    /// </summary>
    /// <seealso cref="DD4T.ViewModels.Base.ViewModelBase" />
    [ContentModel("redirects", true)]
    public class Redirects : ViewModelBase
    {
        /// <summary>
        /// Gets or sets the redirect.
        /// </summary>
        /// <value>The redirect.</value>
        [EmbeddedSchemaField]
        public List<Redirect> Redirect { get; set; } = new List<Redirect>();
    }

    /// <summary>
    /// Class Redirect.
    /// Used for configuring a redirect in Tridion
    /// </summary>
    /// <seealso cref="DD4T.ViewModels.Base.ViewModelBase" />
    [ContentModel("embedRedirect", true)]
    public class Redirect : ViewModelBase
    {
        /// <summary>
        /// Gets or sets to.
        /// </summary>
        /// <value>To.</value>
        [TextField]
        public string To { get; set; }

        /// <summary>
        /// Gets or sets from.
        /// </summary>
        /// <value>From.</value>
        [TextField]
        public string From { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        [TextField]
        public string Type { get; set; }
    }
}