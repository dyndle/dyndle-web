using DD4T.Core.Contracts.ViewModels;
using Dyndle.Modules.Core.Models.Query;


namespace Dyndle.Modules.Core.Providers.Content
{
    /// <summary>
    /// Interface IContentQueryProvider
    /// </summary>
    public interface IContentQueryProvider
    {
        /// <summary>
        /// Queries the specified skip.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="skip">The skip.</param>
        /// <param name="take">The take.</param>
        /// <returns>IEnumerable&lt;T&gt;.</returns>
        string[] Query<T>(int skip, int take) where T : IViewModel;
        /// <summary>
        /// Queries the specified skip.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="skip">The skip.</param>
        /// <param name="take">The take.</param>
        /// <param name="criteria">The criteria.</param>
        /// <param name="singlePublication">Flag to query in current publication.</param>
        /// <returns>IEnumerable&lt;T&gt;.</returns>
        string[] Query<T>(int skip, int take, QueryCriteria criteria, bool singlePublication = true) where T : IViewModel;

    }
}