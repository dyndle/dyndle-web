using Trivident.Modules.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trivident.Modules.Core.Contracts
{
    /// <summary>
    /// Interface IWebPageEnrichmentProvider. Used to identify providers that can enrich web pages. 
    /// Those implementations are automatically called from the <see cref="Trivident.Modules.Core.Controllers.PageController" />
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