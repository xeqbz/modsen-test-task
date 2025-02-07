namespace LibraryApi.Domain.Entities
{
    public class Author
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public DateTimeOffset DateOfBirth { get; set; }
        public List<Book> Books { get; set; } = new();
    }
}
