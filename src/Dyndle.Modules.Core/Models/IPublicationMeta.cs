using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dyndle.Modules.Core.Models
{
    public interface IPublicationMeta
    {
        int Id { get; set; }
        string Title { get; set; }
        string Key { get; set; }
        string PublicationPath { get; set; }
        string PublicationUrl { get; set; }
        string MultimediaPath { get; set; }
        string MultimediaUrl { get; set; }
    }
}
