using System.Text.RegularExpressions;

namespace Trivident.Modules.Core.Providers.Redirection
{
    /// <summary>
    /// Interface IRedirectionDefinition
    /// Used to create different types of redirect definitions
    /// </summary>
    public interface IRedirectionDefinition
    {
        /// <summary>
        /// Determines whether the specified page URL is match.
        /// </summary>
        /// <param name="pageUrl">The page URL.</param>
        /// <returns><c>true</c> if the specified page URL is match; otherwise, <c>false</c>.</returns>
        bool IsMatch(string pageUrl);

        /// <summary>
        /// Gets to.
        /// </summary>
        /// <value>To.</value>
        string To { get; }
        /// <summary>
        /// Gets a value indicating whether this instance is permanent.
        /// </summary>
        /// <value><c>true</c> if this instance is permanent; otherwise, <c>false</c>.</value>
        bool IsPermanent { get; }
    }
}