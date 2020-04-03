using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DD4T.ContentModel.Contracts.Configuration;
using DD4T.ContentModel.Contracts.Logging;
using Dyndle.Modules.Core.Controllers.Base;
using Dyndle.Modules.Core.Extensions;
using Dyndle.Modules.Management.Contracts;
using Dyndle.Modules.Management.Models;

namespace Dyndle.Modules.Management.Controllers
{
    /// <summary>
    /// Class PageController.
    /// Used to handle all default page requests
    /// </summary>
    /// <seealso cref="ModuleControllerBase" />
    public class CacheController : Controller
    {

        /// <summary>
        /// The cache provider
        /// </summary>
        private readonly ICacheProvider _cacheProvider;

        private readonly int PAGESIZE = 20;


        /// <summary>
        /// Initializes a new instance of the <see cref="CacheController"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="logger">The logger.</param>
        public CacheController(ICacheProvider cacheProvider, ILogger logger, IDD4TConfiguration configuration)
        {
            cacheProvider.ThrowIfNull(nameof(cacheProvider));
            logger.ThrowIfNull(nameof(logger));
            configuration.ThrowIfNull(nameof(configuration));

            _cacheProvider = cacheProvider;
        }

        /// <summary>
        /// List all items in the cache
        /// </summary>
        /// <returns>ActionResult.</returns>
        public virtual ActionResult List(int pageNr = 0, string search = "")
        {
            CacheListInfo cli = new CacheListInfo();

            cli.Items = _cacheProvider.GetList();
            if (! string.IsNullOrEmpty(search))
            {
                cli.Items = _cacheProvider.GetList().Where(i => i.Key.ToLowerInvariant().Contains(search.ToLowerInvariant()));
                cli.SearchQuery = search;
            }
            cli.Total = cli.Items.Count();
            if (pageNr >= 0)
            {
               
                cli.CurrentPageNr = pageNr;
                cli.HasPrevious = cli.CurrentPageNr > 0;
                if (cli.Total > pageNr * PAGESIZE)
                {
                    cli.Items = cli.Items.Skip((pageNr) * PAGESIZE);
                    if (cli.Items.Count() >= (pageNr + 1) * PAGESIZE)
                    {
                        cli.HasNext = true;
                        cli.Items = cli.Items.Take(PAGESIZE);
                    }
                }
                else
                {
                    ViewBag.Message = "No more results";
                    cli.Items = new List<CacheItem>();
                }
                cli.PaginationEnabled = cli.HasNext || cli.HasPrevious;
                cli.CurrentPageSize = cli.Items.Count();
                cli.NrOfPages = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(cli.Total) / Convert.ToDouble(PAGESIZE)));
            }
            else
            {
                cli.PaginationEnabled = false;
                cli.CurrentPageSize = cli.Items.Count();
            }
            return View(cli);
        }


        /// <summary>
        /// Get item from the cache
        /// </summary>
        /// <returns>ActionResult.</returns>
        public virtual ActionResult Item(string key)
        {
            return View(_cacheProvider.GetItem(key.Base64Decode()));
        }


        /// <summary>
        /// Remove item from the cache
        /// </summary>
        /// <returns>ActionResult.</returns>
        public virtual ActionResult Remove(string key)
        {
            _cacheProvider.RemoveItem(key.Base64Decode());
            return View(new CacheItem() { Key = key });
        }

        /// <summary>
        /// Remove all items from cache
        /// </summary>
        /// <returns>ActionResult.</returns>
        public virtual ActionResult RemoveAll()
        {
            _cacheProvider.ClearCache();
            return View();
        }
    }
}