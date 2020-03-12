
using System.Text.RegularExpressions;
using System.Web;

namespace Trivident.Modules.Core.Providers.Redirection
{
    /// <summary>
    /// Class RegExRedirectionDefinition.
    /// Implements redirect matching based on an regular expression match based on the URL
    /// </summary>
    /// <seealso cref="Trivident.Modules.Core.Providers.Redirection.IRedirectionDefinition" />
    public class RegExRedirectionDefinition : IRedirectionDefinition
    {
        private readonly Regex _regex;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegExRedirectionDefinition"/> class.
        /// </summary>
        /// <param name="pattern">The pattern.</param>
        /// <param name="to">To.</param>
        /// <param name="isPermanent">if set to <c>true</c> [is permanent].</param>
        public RegExRedirectionDefinition(string pattern, string to, bool isPermanent)
        {
            _regex = new Regex(pattern, RegexOptions.IgnoreCase);

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
            return _regex.IsMatch(pageUrl);
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