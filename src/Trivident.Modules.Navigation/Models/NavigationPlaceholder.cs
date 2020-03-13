using DD4T.ViewModels.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dyndle.Modules.Core.Models;

namespace Trivident.Modules.Navigation.Models
{
    /// <summary>
    /// Class NavigationPlaceholder.
    /// Used for mapping the navigation component as a placeholder. 
    /// All relevant information to render the navigation component comes from the component template meta data 
    /// This is handled by the <see cref="Trivident.Modules.Navigation.Binders.SitemapItemModelBinder"/> 
    /// </summary>
    /// <seealso cref="EntityModel" />
    [ContentModel("navigationPlaceholder", true)]
    public class NavigationPlaceholder : EntityModel
    {
    }
}