using System;
using System.Collections.Generic;
using DD4T.ContentModel.Contracts.Caching;
using DD4T.ContentModel.Contracts.Resolvers;
using DD4T.Core.Contracts.ViewModels;
using Dyndle.Modules.Core.Contracts;
using Dyndle.Modules.Core.Extensions;
using Dyndle.Modules.Core.Interfaces;
using Dyndle.Modules.Core.Models.Query;
using Dyndle.Modules.Core.Providers.Content;

namespace Dyndle.Modules.Core.Services
{
    public class ContentQueryService : IContentQueryService
    {
        // cache keys
        private const string CacheKeyFormat = "ContentQuery_{0}";
        private const string CacheRegion = "ContentQuery";

        private readonly IContentProvider _contentProvider;
        private readonly IPublicationResolver _publicationResolver;
        private readonly ICacheAgent _cacheAgent;
        private readonly IContentQueryProvider _contentQueryProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultContentQueryProvider" /> class.
        /// </summary>
        /// <param name="contentProvider">The content provider.</param>
        /// <param name="publicationResolver">The publication resolver.</param>
        /// <param name="cacheAgent">The cache agent.</param>
        public ContentQueryService(IContentProvider contentProvider, IPublicationResolver publicationResolver, ICacheAgent cacheAgent, IContentQueryProvider contentQueryProvider)
        {
            cacheAgent.ThrowIfNull(nameof(cacheAgent));
            contentProvider.ThrowIfNull(nameof(contentProvider));
            publicationResolver.ThrowIfNull(nameof(publicationResolver));

            _cacheAgent = cacheAgent;
            _contentQueryProvider = contentQueryProvider;
            _publicationResolver = publicationResolver;
            _contentProvider = contentProvider;
        }

        /// <summary>
        /// Queries the broker for dynamic components or pages.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="skip">The skip.</param>
        /// <param name="take">The take.</param>
        /// <returns>IEnumerable&lt;T&gt;.</returns>
        public IEnumerable<T> Query<T>(int skip, int take) where T : IViewModel
        {
            return Query<T>(skip, take, new QueryCriteria());
        }

        /// <summary>
        /// Queries the broker for dynamic components or pages.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="skip">The skip.</param>
        /// <param name="take">The take.</param>
        /// <param name="criteria">The criteria.</param>
        /// <returns>IEnumerable&lt;T&gt;.</returns>
        /// <exception cref="Exception">ViewName {0} not found.".FormatString(criteria.ViewTitle)</exception>
        /// <exception cref="InvalidCastException"></exception>
        public IEnumerable<T> Query<T>(int skip, int take, QueryCriteria criteria) where T : IViewModel
        {
            var publicationId = _publicationResolver.ResolvePublicationId();

            string cacheKey = string.Format(CacheKeyFormat, string.Join("|", typeof(T).FullName, skip, take, publicationId, criteria.GetUniqueKey()));

            var result = (List<T>)_cacheAgent.Load(cacheKey);

            if (result != null)
            {
                return result;
            }

            var allComponentUris = _contentQueryProvider.Query<T>(skip, take, criteria);

            result = new List<T>();

            foreach (var item in allComponentUris)
            {
                var viewModel = _contentProvider.BuildViewModel(new TcmUri(item));
                if (viewModel == null)
                    continue;

                if (!(viewModel is T))
                    throw new InvalidCastException(string.Format("Query for '{1}' returns items of the wrong type '{0}'", viewModel.GetType().FullName, typeof(T).FullName));

                result.Add((T)viewModel);
            }

            _cacheAgent.Store(cacheKey, CacheRegion, result);

            return result;
        }

    }
}
