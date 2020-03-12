using System.Web.Mvc;

namespace Trivident.Modules.Core.Services.Redirection
{
    /// <summary>
    /// Interface IRedirectionService
    /// Describes a service to use for redirection 
    /// </summary>
    public interface IRedirectionService
    {
        /// <summary>
        /// Tries to redirect the current URL.
        /// </summary>
        /// <param name="pageUrl">The page URL.</param>
        /// <param name="cleanUrl"><c>true</c> if the URL should be cleaned; otherwise, <c>false</c></param>
        /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
        bool TryToRedirect(string pageUrl, bool cleanUrl);
        /// <summary>
        /// Gets the redirect result.
        /// </summary>
        /// <param name="pageUrl">The page URL.</param>
        /// <param name="cleanUrl"><c>true</c> if the URL should be cleaned; otherwise, <c>false</c></param>
        /// <returns>ActionResult.</returns>
        ActionResult GetRedirectResult(string pageUrl, bool cleanUrl);
    }
}