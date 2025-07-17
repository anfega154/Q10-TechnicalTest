using Microsoft.EntityFrameworkCore;

namespace Q10_TechnicalTest.Utils;
public class PaginatedList<T> : List<T>, IPaginatedList
{
    public int PageIndex { get; private set; }
    public int TotalPages { get; private set; }
    public string SearchString { get; set; }

    public PaginatedList(List<T> items, int count, int pageIndex, int pageSize, string searchString = null)
    {
        PageIndex = pageIndex;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        SearchString = searchString;
        this.AddRange(items);
    }

    public bool HasPreviousPage => PageIndex > 1;
    public bool HasNextPage => PageIndex < TotalPages;

    public static async Task<PaginatedList<T>> CreateAsync(
        IQueryable<T> source, int pageIndex, int pageSize, string searchString = null)
    {
        var count = await source.CountAsync();
        var items = await source.Skip((pageIndex - 1) * pageSize)
                              .Take(pageSize)
                              .ToListAsync();
        return new PaginatedList<T>(items, count, pageIndex, pageSize, searchString);
    }
}