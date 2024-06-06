using AutoMapper;
using BookBarn.Application.DTOs.Book;
using BookBarn.Application.DTOs.Cart;
using BookBarn.Domain.Entities;

namespace BookBarn.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Book, BookDto>().ReverseMap();
            CreateMap<Book, CreateBookDto>().ReverseMap();
            CreateMap<Book, UpdateBookDto>().ReverseMap();
            CreateMap<Cart, CartDto>();
            CreateMap<CreateCartDto, Cart>();
            CreateMap<UpdateCartDto, Cart>();
        }
    }
}
