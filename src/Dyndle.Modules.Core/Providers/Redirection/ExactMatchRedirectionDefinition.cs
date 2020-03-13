using System.Web;

namespace Dyndle.Modules.Core.Providers.Redirection
{
    /// <summary>
    /// Class ExactMatchRedirectionDefinition.
    /// Implements redirect matching based on an exact match based on the URL
    /// </summary>
    /// <seealso cref="IRedirectionDefinition" />
    public class ExactMatchRedirectionDefinition : IRedirectionDefinition
    {
        private readonly string _from;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExactMatchRedirectionDefinition"/> class.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="isPermanent">if set to <c>true</c> [is permanent].</param>
        public ExactMatchRedirectionDefinition(string from, string to, bool isPermanent)
        {
            _from = from;
            To = to;
            IsPermanent = isPermanent;
        }
        /// <summary>
        /// Determines whether the specified page URL is match.
        /// </summary>
        /// <param name="pageUrl">The page URL.</param>
        /// <returns><c>true</c> if the specified page URL is match; otherwise, <c>false</c>.</returns>
        public bool IsMatch(string pageUrl)
        {
            pageUrl = HttpUtility.UrlDecode(pageUrl);
            return _from.Equals(pageUrl, System.StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Gets the URL to redirect to.
        /// </summary>
        /// <value>To.</value>
        public string To { get; set; }
        /// <summary>
        /// Gets a value indicating whether this redirect is permanent.
        /// </summary>
        /// <value><c>true</c> if this redirect is permanent; otherwise, <c>false</c>.</value>
        public bool IsPermanent { get; set; }

    }
}