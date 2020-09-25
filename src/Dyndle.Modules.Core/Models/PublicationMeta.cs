using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dyndle.Modules.Core.Models
{
    public class PublicationMeta : IPublicationMeta
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Key { get; set; }
        public string PublicationPath { get; set; }
        public string PublicationUrl { get; set; }
        public string MultimediaPath { get; set; }
        public string MultimediaUrl { get; set; }
        public Dictionary<string, string> CustomMeta { get; set; }
    }
}
