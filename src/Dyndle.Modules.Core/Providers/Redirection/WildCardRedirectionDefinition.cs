using System.Text.RegularExpressions;

namespace Dyndle.Modules.Core.Providers.Redirection
{
    /// <summary>
    /// Class WildCardRedirectionDefinition.
    /// Implements redirect matching using wildcard matching based on the URL
    /// </summary>
    /// <seealso cref="RegExRedirectionDefinition" />
    public class WildCardRedirectionDefinition : RegExRedirectionDefinition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WildCardRedirectionDefinition"/> class.
        /// </summary>
        /// <param name="wildCard">The wild card.</param>
        /// <param name="to">To.</param>
        /// <param name="isPermanent">if set to <c>true</c> [is permanent].</param>
        public WildCardRedirectionDefinition(string wildCard, string to, bool isPermanent)
        : base("^" + Regex.Escape(wildCard)
                              .Replace(@"\*", ".*")
                              .Replace(@"\?", ".")
                       + "$", to, isPermanent)
        {
        }
    }
}