using System;
using System.Collections.Generic;
using DD4T.Core.Contracts.ViewModels;
using Dyndle.Modules.Core.Models.Query;

namespace Dyndle.Modules.Core.Contracts
{
    public interface IContentQueryService
    {
        /// <summary>
        /// Queries the broker for dynamic components or pages.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="skip">The skip.</param>
        /// <param name="take">The take.</param>
        /// <returns>IEnumerable&lt;T&gt;.</returns>
        IEnumerable<T> Query<T>(int skip, int take) where T : IViewModel;

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
        IEnumerable<T> Query<T>(int skip, int take, QueryCriteria criteria) where T : IViewModel;
    }
}