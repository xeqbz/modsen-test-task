using LibraryApi.Application.DTOs;
using LibraryApi.Application.Interfaces;
using LibraryApi.Domain.Entities;
using LibraryApi.Domain.Interfaces;
using AutoMapper;
using LibraryApi.Application.Exceptions;

namespace LibraryApi.Application.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;

        public AuthorService(IAuthorRepository authorRepository, IMapper mapper)
        {
            _authorRepository = authorRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AuthorDTO>> GetAllAuthorsAsync()
        {
            var authors = await _authorRepository.GetAllAsync()
                ?? throw new NotFoundException("No authors found");
            return _mapper.Map<IEnumerable<AuthorDTO>>(authors);
        }

        public async Task<AuthorDTO?> GetAuthorByIdAsync(int id)
        {
            var author = await _authorRepository.GetByIdAsync(id)
                ?? throw new NotFoundException("Author not found");
            return _mapper.Map<AuthorDTO>(author);
        }

        public async Task<AuthorDTO> CreateAuthorAsync(AuthorDTO dto)
        {
            var author = _mapper.Map<Author>(dto);
            await _authorRepository.AddAsync(author);
            return _mapper.Map<AuthorDTO>(author);
        }

        public async Task<bool> UpdateAuthorAsync(int id, AuthorDTO dto)
        {
            var author = await _authorRepository.GetByIdAsync(id)
                ?? throw new NotFoundException("Author not found");
            _mapper.Map(dto, author);
            await _authorRepository.UpdateAsync(author);
            return true;
        }

        public async Task<bool> DeleteAuthorAsync(int id)
        {
            var author = await _authorRepository.GetByIdAsync(id)
                ?? throw new NotFoundException("Author not found");
            await _authorRepository.DeleteAsync(author);
            return true;
        }
    }
}
