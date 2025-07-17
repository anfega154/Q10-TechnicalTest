namespace Q10_TechnicalTest.Utils;

public interface IPaginatedList
{
    int PageIndex { get; }
    int TotalPages { get; }
    bool HasPreviousPage { get; }
    bool HasNextPage { get; }
    string SearchString { get; }
}