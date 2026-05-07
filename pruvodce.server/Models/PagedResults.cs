namespace pruvodce.server.Models
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; } = new();

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalItems { get; set; }

        public int TotalPages => (int)Math.Ceiling(TotalItems / (double)PageSize);

        public int FirstItemNumber =>
            TotalItems == 0 ? 0 : ((PageNumber - 1) * PageSize) + 1;

        public int LastItemNumber =>
            Math.Min(PageNumber * PageSize, TotalItems);

        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
    }
}