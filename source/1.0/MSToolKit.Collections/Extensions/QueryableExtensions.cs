using System.Linq;

namespace MSToolKit.Collections.Extensions
{
    public static class QueryableExtensions
    {
        /// <summary>
        /// Returns new instance of PaginatedList with the given data.
        /// </summary>
        /// <typeparam name="T">
        /// The generic type, that should be paginated.
        /// </typeparam>
        /// <param name="source">System.Linq.IQueryable with all items.</param>
        /// <param name="pageIndex">The current page index, that should be returned.</param>
        /// <param name="itemsPerPage">The count of items per page, that should be returned.</param>
        /// <returns>
        /// New instance for MSToolKit.Collections.Abstraction.IPaginatedList with the given data.
        /// </returns>
        public static PaginatedList<T> ToPaginatedList<T>(
            this IQueryable<T> source, int pageIndex, int itemsPerPage)
            where T : class
        {
            return new PaginatedList<T>(source, pageIndex, itemsPerPage);
        }
    }
}
