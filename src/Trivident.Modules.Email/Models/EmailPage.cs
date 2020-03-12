using DD4T.ViewModels.Attributes;
using Trivident.Modules.Core.Models;
using System.Collections.Generic;

namespace Trivident.Modules.Email.Models
{
    [PageViewModel(TemplateTitle = "Email Page")]
    public class EmailPage : WebPage
    {
        [PresentationsByRegion(Region = "Header")]
        public List<IEntityModel> Header { get; set; }

        [PresentationsByRegion(Region = "Detail")]
        public List<IEntityModel> Detail { get; set; }

        [PresentationsByRegion(Region = "Main")]
        public List<IEntityModel> Main { get; set; }

        [PresentationsByRegion(Region = "Footer")]
        public List<IEntityModel> Footer { get; set; }
    }
}