using AutoMapper;
using LibraryApi.Application.DTOs;
using LibraryApi.Domain.Entities;

namespace LibraryApi.Application.Mappings
{
    public class AuthorProfile : Profile
    {
        public AuthorProfile()
        {
            CreateMap<Author, AuthorDTO>();
            CreateMap<AuthorDTO, Author>();
        }
    }
}
