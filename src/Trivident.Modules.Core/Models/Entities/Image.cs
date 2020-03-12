using DD4T.ViewModels.Attributes;
using Trivident.Modules.Core.Contracts.Entities;

namespace Trivident.Modules.Core.Models.Entities
{
    [ContentModel("Image", true)]
    public class Image : EntityModel, IMedia
    {
        public new string Url { get { return "https://placeholdit.imgix.net/~text?txtsize=100&txt=Image&bg=D3D3D3&txtcolor=fefefe"; } }
    }
}