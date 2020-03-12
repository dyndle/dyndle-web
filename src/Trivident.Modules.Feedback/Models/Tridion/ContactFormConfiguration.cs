using System.Collections.Generic;
using System.Linq;
using DD4T.ViewModels.Attributes;
using Trivident.Modules.Core.Models;
using Trivident.Modules.Feedback.Contracts;

namespace Trivident.Modules.Feedback.Models.Tridion
{
    /// <summary>
    /// Defines the schema fields in Tridion for this entity
    /// </summary>
    [ContentModel("ContactForm", true)]
    public class ContactFormConfiguration : EntityModel, IContactFormDataProvider
    {
        [TextField(FieldName = "Subjects")]
        public List<string> SubjectValues { get; set; } = new List<string>();

        /// <summary>
        /// Generate list of dropdown options
        /// </summary>
        public IList<KeyValuePair<string, string>> Subjects
        {
            get { return SubjectValues.Select(s => new KeyValuePair<string, string>(s, s)).ToList(); }
        }

    }
}