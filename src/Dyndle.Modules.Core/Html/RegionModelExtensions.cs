using System.Collections.Generic;
using System.Linq;
using Dyndle.Modules.Core.Models;

namespace Dyndle.Modules.Core.Html
{
    /// <summary>
    /// Class RegionModelExtensions.
    /// Provides additional functionality for regions
    /// </summary>
    public static class RegionModelExtensions
    {
        /// <summary>
        /// Groups the entities into rows.
        /// Row sizes are calculated based on region grid size and entity grid size
        /// </summary>
        /// <param name="regionModel">The region model.</param>
        /// <returns>IEnumerable&lt;IEnumerable&lt;IEntityModel&gt;&gt;.</returns>
        public static IEnumerable<IEnumerable<IEntityModel>> GetRows(this IRegionModel regionModel)
        {
            Queue<IEntityModel> queue = new Queue<IEntityModel>(regionModel.Entities);
            while (queue.Any())
            {
                yield return GetRow(regionModel, queue);
            }
        }

        /// <summary>
        /// Gets a single row from the queue calculated based on region grid size and entity grid size
        /// </summary>
        /// <param name="regionModel">The region model.</param>
        /// <param name="queue">The queue.</param>
        /// <returns>IEnumerable&lt;IEntityModel&gt;.</returns>
        private static IEnumerable<IEntityModel> GetRow(this IRegionModel regionModel, Queue<IEntityModel> queue)
        {
            var remaining = regionModel.GridSize;
            IEntityModel entity;
            while (queue.Any())
            {
                entity = queue.Peek();
                remaining = remaining - entity.MvcData.EntityGridSize;

                if (remaining >= 0)
                {
                    yield return queue.Dequeue();
                }
                else
                {
                    break;
                }
            }
        }
    }
}