using AutoMapper;
using BookBarn.Domain.Entities;

namespace BookBarn.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            //CreateMap<Book, BookDto>().ReverseMap();
            //CreateMap<Book, CreateBookDto>().ReverseMap();
            //CreateMap<Book, UpdateBookDto>().ReverseMap();
        }
    }
}
