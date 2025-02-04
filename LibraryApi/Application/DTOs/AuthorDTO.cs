using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace LibraryApi.Application.DTOs
{
    public class AuthorDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public DateTimeOffset DateOfBirth { get; set; }
    }
}
