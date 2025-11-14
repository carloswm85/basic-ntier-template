using Microsoft.EntityFrameworkCore;

namespace BasicNtierTemplate.Service.Dtos
{
    /// <summary>
    /// Creates a paginated subset of an <see cref="IQueryable{T}"/> source.
    /// <br/><br/>
    /// <b>How it works:</b><br/>
    /// The <see cref="CreateAsync(IQueryable{T}, int, int)"/> method applies 
    /// <see cref="Queryable.Skip{TSource}(IQueryable{TSource}, int)"/> and 
    /// <see cref="Queryable.Take{TSource}(IQueryable{TSource}, int)"/> 
    /// to retrieve only the records for the selected page.
    /// <br/><br/>
    /// When <see cref="EntityFrameworkQueryableExtensions.ToListAsync{TSource}(IQueryable{TSource}, CancellationToken)"/> 
    /// is executed, it returns a <see cref="List{T}"/> containing just the items 
    /// for that specific page.  
    /// <br/><br/>
    /// The properties <see cref="HasPreviousPage"/> and <see cref="HasNextPage"/> 
    /// are convenient helpers for enabling or disabling UI paging buttons.
    /// <br/><br/>
    /// A static CreateAsync method is used instead of a constructor because 
    /// constructors cannot execute asynchronous code.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the paginated collection.</typeparam>
    public class PaginatedList<T> : List<T>
    {
        /// <summary>
        /// Current page number (1-based).
        /// </summary>
        public int PageIndex { get; private set; }

        /// <summary>
        /// Total number of pages available.
        /// </summary>
        public int TotalPages { get; private set; }

        /// <summary>
        /// Creates a PaginatedList with the provided items and paging metadata.
        /// </summary>
        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        /// <summary>
        /// True if this list has a previous page.
        /// </summary>
        public bool HasPreviousPage => PageIndex > 1;

        /// <summary>
        /// True if this list has a next page.
        /// </summary>
        public bool HasNextPage => PageIndex < TotalPages;

        /// <summary>
        /// Asynchronously creates a <see cref="PaginatedList{T}"/> by applying
        /// paging logic (<c>Skip</c> and <c>Take</c>) to the given source.
        /// </summary>
        /// <param name="source">The queryable source to paginate.</param>
        /// <param name="pageIndex">The current page number (1-based).</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <returns>
        /// A <see cref="Task{TResult}"/> whose result is a fully populated
        /// <see cref="PaginatedList{T}"/> instance containing:
        /// <br/>• The current page of data  
        /// <br/>• The total item count  
        /// <br/>• Page index and total pages statistics  
        /// </returns>
        public static async Task<PaginatedList<T>> CreateAsync(
            IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }
}
