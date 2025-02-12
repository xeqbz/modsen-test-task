using LibraryApi.Domain.Entities;
using AutoMapper;
using LibraryApi.Application.DTOs;
using LibraryApi.Application.Requests;

namespace LibraryApi.Application.Mappings
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            CreateMap<Book, BookDTO>()
                .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author.FirstName + " " + src.Author.LastName));
            CreateMap<CreateBookRequest, Book>();
        }
    }
}
