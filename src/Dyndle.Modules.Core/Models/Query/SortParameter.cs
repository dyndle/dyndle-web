namespace Dyndle.Modules.Core.Models.Query
{
    /// <summary>
    /// Class SortParameter.
    /// </summary>
    public class SortParameter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SortParameter"/> class.
        /// </summary>
        /// <param name="sortColumn">The sort column.</param>
        /// <param name="sortDirection">The sort direction.</param>
        public SortParameter(SortColumn sortColumn, SortDirection sortDirection)
        {
            SortColumn = sortColumn;
            SortDirection = sortDirection;
        }
        /// <summary>
        /// Gets or sets the sort column.
        /// </summary>
        /// <value>The sort column.</value>
        public SortColumn SortColumn { get; set; }
        /// <summary>
        /// Gets or sets the sort direction.
        /// </summary>
        /// <value>The sort direction.</value>
        public SortDirection SortDirection { get; set; }
    }
}