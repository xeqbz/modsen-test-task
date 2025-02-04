using LibraryApi.Application.DTOs;

namespace LibraryApi.Application.Interfaces
{
    public interface IAuthorService
    {
        Task<IEnumerable<AuthorDTO>> GetAllAuthorsAsync();
        Task<AuthorDTO?> GetAuthorByIdAsync(int id);
        Task<AuthorDTO> CreateAuthorAsync(AuthorDTO dto);
        Task<bool> UpdateAuthorAsync(int id, AuthorDTO dto);
        Task<bool> DeleteAuthorAsync(int id);

    }
}
