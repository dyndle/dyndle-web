using DD4T.ViewModels.Attributes;
using Trivident.Modules.Core.Contracts.Entities;

namespace Trivident.Modules.Core.Models.Entities
{
    [ContentModel("Video", true)]
    public class Video : EntityModel, IMedia
    {
        [MultimediaUrl]
        public new string Url { get; set; }
    }
}