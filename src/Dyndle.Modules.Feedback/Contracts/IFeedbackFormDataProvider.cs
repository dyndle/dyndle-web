using System.Collections.Generic;

namespace Dyndle.Modules.Feedback.Contracts
{
    /// <summary>
    /// Provider for feedback form dropdown options
    /// </summary>
    public interface IFeedbackFormDataProvider
    {
        IList<KeyValuePair<string, string>> Titles { get; }

        IList<KeyValuePair<string, string>> Countries { get; }

        IList<KeyValuePair<string, string>> AgeRanges { get; }

        IList<KeyValuePair<string, string>> Products(int siteId);

        IList<KeyValuePair<string, string>> Reasons { get; }
    }
}
