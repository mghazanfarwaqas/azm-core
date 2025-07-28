using System.Globalization;
using Microsoft.EntityFrameworkCore;

namespace Azm.Core.Utils
{
    public class PagedList<T> 
    {
        private readonly IQueryable<T> _source;
        private readonly IEnumerable<T> _memSource;

        public PagedList(IQueryable<T> source, int page, int pageSize)
        {
            _source = source;
            CurrentPage = page;
            CurrentPageIndex = page < 1 ? 0 : page - 1;
            PageSize = pageSize;
        }

        public PagedList(IEnumerable<T> source, int page, int pageSize)
        {
            _memSource = source;
            CurrentPage = page;
            CurrentPageIndex = page < 1 ? 0 : page - 1;
            PageSize = pageSize;
        }

        public void GetMemoryPagedResults()
        {
            TotalRecords = _memSource.Count();
            Items = _memSource.Skip(CurrentPageIndex * PageSize).Take(PageSize).ToList();
            TotalPages = (int)Math.Ceiling(Convert.ToDecimal(TotalRecords, CultureInfo.InvariantCulture) / PageSize);
        }

        public async Task GetPagedResults()
        {
            TotalRecords = await _source.CountAsync();
            Items = await _source.Skip(CurrentPageIndex * PageSize).Take(PageSize).ToListAsync();
            TotalPages = (int)Math.Ceiling(Convert.ToDecimal(TotalRecords, CultureInfo.InvariantCulture) / PageSize);
        }

        public int CurrentPageIndex { get; private set; }
        public int CurrentPage { get; private set; }
        public int PageSize { get; private set; }
        public int TotalRecords { get; private set; }
        public int TotalPages { get; private set; }
        public bool OnFirstPage { get { return CurrentPageIndex == 0; } }
        public bool OnLastPage { get { return (CurrentPageIndex + 1) == TotalPages; } }
        public bool HasNextPage { get { return (CurrentPageIndex + 1) < TotalPages; } }
        public bool HasPreviousPage { get { return CurrentPageIndex > 0; } }
        public IEnumerable<T> Items { get; private set; }
    }
}
