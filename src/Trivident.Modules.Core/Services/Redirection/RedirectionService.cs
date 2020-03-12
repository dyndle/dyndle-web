using Trivident.Modules.Core.Providers.Redirection;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Trivident.Modules.Core.Services.Redirection
{
    /// <summary>
    /// Class RedirectionService.
    /// Implementation for redirection based on redirect definitions from <see cref="IRedirectionDefinitionProvider"/> 
    /// </summary>
    /// <seealso cref="Trivident.Modules.Core.Services.Redirection.IRedirectionService" />
    public class RedirectionService : IRedirectionService
    {
        private readonly IRedirectionDefinitionProvider _redirectionDefinitionProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="RedirectionService"/> class.
        /// </summary>
        /// <param name="redirectionDefinitionProvider">The redirection definition provider.</param>
        public RedirectionService(IRedirectionDefinitionProvider redirectionDefinitionProvider)
        {
            redirectionDefinitionProvider.ThrowIfNull(nameof(redirectionDefinitionProvider));
            _redirectionDefinitionProvider = redirectionDefinitionProvider;
        }

        /// <summary>
        /// Tries to redirect the current URL.
        /// </summary>
        /// <param name="pageUrl">The page URL.</param>
        /// <param name="cleanUrl"><c>true</c> if the URL should be cleaned; otherwise, <c>false</c></param>
        /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
        public bool TryToRedirect(string pageUrl, bool cleanUrl)
        {
            IRedirectionDefinition matchingDefinition;
            var result = TryGetDefinition(pageUrl, cleanUrl, out matchingDefinition);

            if (result)
            {
                if (matchingDefinition.IsPermanent)
                {
                    HttpContext.Current.Response.RedirectPermanent(matchingDefinition.To);
                }
                else
                {
                    HttpContext.Current.Response.Redirect(matchingDefinition.To);
                }
            }

            return result;
        }

        /// <summary>
        /// Tries the get a matching definition from the provider.
        /// </summary>
        /// <param name="pageUrl">The page URL.</param>
        /// <param name="cleanUrl"><c>true</c> if the URL should be cleaned; otherwise, <c>false</c></param>
        /// <param name="definition">The definition.</param>
        /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
        private bool TryGetDefinition(string pageUrl, bool cleanUrl, out IRedirectionDefinition definition)
        {
            definition = _redirectionDefinitionProvider.GetDefinitions(cleanUrl).FirstOrDefault(d => d.IsMatch(pageUrl));

            return !definition.IsNull();
        }

        /// <summary>
        /// Gets the redirect result if a match is found.
        /// </summary>
        /// <param name="pageUrl">The page URL.</param>
        /// <param name="cleanUrl"><c>true</c> if the URL should be cleaned; otherwise, <c>false</c></param>
        /// <returns>ActionResult.</returns>
        public ActionResult GetRedirectResult(string pageUrl, bool cleanUrl)
        {
            IRedirectionDefinition matchingDefinition;

            if (TryGetDefinition(pageUrl, cleanUrl, out matchingDefinition))
            {
                return new RedirectResult(matchingDefinition.To, matchingDefinition.IsPermanent);
            }
            else 
            {
                return null;
            }
        }
    }
}
