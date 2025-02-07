namespace LibraryApi.Application.DTOs
{
    public class BookDTO
    {
        public int Id { get; set; }
        public string ISBN { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string AuthorName { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;
    }
}
