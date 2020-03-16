using DD4T.ViewModels.Attributes;
using Dyndle.Modules.Core.Models;
using Dyndle.Modules.Navigation.Binders;

namespace Dyndle.Modules.Navigation.Models
{
    /// <summary>
    /// Class NavigationPlaceholder.
    /// Used for mapping the navigation component as a placeholder. 
    /// All relevant information to render the navigation component comes from the component template meta data 
    /// This is handled by the <see cref="SitemapItemModelBinder"/> 
    /// </summary>
    /// <seealso cref="EntityModel" />
    [ContentModel("navigationPlaceholder", true)]
    public class NavigationPlaceholder : EntityModel
    {
    }
}