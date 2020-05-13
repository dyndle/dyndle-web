using Dyndle.Modules.Core.Controllers;
using Dyndle.Modules.Core.Models;

namespace Dyndle.Modules.Core.Contracts
{
    /// <summary>
    /// Interface IWebPageEnrichmentProvider. Used to identify providers that can enrich web pages. 
    /// Those implementations are automatically called from the <see cref="PageController" />
    /// </summary>
    public interface IWebPageEnrichmentProvider
    {
        /// <summary>
        /// Enriches the web page.
        /// </summary>
        /// <param name="webPage">The web page.</param>
        void EnrichWebPage(IWebPage webPage);
    }
}