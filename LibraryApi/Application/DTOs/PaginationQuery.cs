namespace LibraryApi.Application.DTOs
{
    public class PaginationQuery
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public PaginationQuery()
        {
        }

        public PaginationQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber < 1 ? 1 : pageNumber;
            PageSize = pageSize > 50 ? 50 : pageSize;
        }
    }
}
