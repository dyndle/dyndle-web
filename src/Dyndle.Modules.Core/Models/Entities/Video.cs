using DD4T.ViewModels.Attributes;
using Dyndle.Modules.Core.Contracts.Entities;

namespace Dyndle.Modules.Core.Models.Entities
{
    [ContentModel("Video", true)]
    public class Video : EntityModel, IMedia
    {
        [MultimediaUrl]
        public new string Url { get; set; }
    }
}