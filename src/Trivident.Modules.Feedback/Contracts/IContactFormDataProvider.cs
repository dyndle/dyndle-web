using System.Collections.Generic;

namespace Trivident.Modules.Feedback.Contracts
{
    /// <summary>
    /// Configuration for the contact form subjects dropdown 
    /// </summary>
    public interface IContactFormDataProvider
    {
        IList<KeyValuePair<string, string>> Subjects { get; }
    }
}
