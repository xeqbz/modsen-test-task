namespace LibraryApi.Domain.Common;

public class PagedResponse<T>
{
    public IEnumerable<T> Data { get; set; } = new List<T>();
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }

    public PagedResponse(IEnumerable<T> data, int pageNumber, int pageSize, int totalItems)
    {
        Data = data;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalItems = totalItems;
    }
}
