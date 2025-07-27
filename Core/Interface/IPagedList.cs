namespace Core.Interface
{
    public interface IPagedList<T>
    {
        int CurrentPage { get; }
        int PageSize { get; }
        int TotalRecords { get; }
        int TotalPages { get; }
        bool OnFirstPage { get; }
        bool OnLastPage { get; }
        bool HasNextPage { get; }
        bool HasPreviousPage { get; }

        IEnumerable<T> Items { get; }
    }
}
