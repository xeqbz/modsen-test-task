namespace LibraryApi.Domain.Entities
{
    public class Book
    {
        public int Id { get; set; }
        public string ISBN { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int AuthorId { get; set; }
        public Author Author { get; set; } = null!;
        public string? ImagePath { get; set; }

        public DateTimeOffset? IssuedAt { get; set; }
        public DateTimeOffset? DueTo { get; set; }
    }
}
