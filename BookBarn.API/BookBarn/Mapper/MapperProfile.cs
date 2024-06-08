using AutoMapper;
using BookBarn.Application.DTOs.Auth;
using BookBarn.Application.DTOs.Book;
using BookBarn.Application.DTOs.Cart;
using BookBarn.Application.DTOs.Checkout;
using BookBarn.Application.DTOs.User;
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
            CreateMap<AppUser, UserResponseDto>();
            CreateMap<AppUser, UserResponseDto>()
                .ForMember(dest => dest.Role, opt => opt.Ignore());
            CreateMap<AppUser, UserResponseDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl))
            .ForMember(dest => dest.DateModified, opt => opt.MapFrom(src => src.DateModified));
            CreateMap<RegisterUserDto, AppUser>();
            CreateMap<RegisterUserDto, AppUser>()
           .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));
            CreateMap<Checkout, CheckoutResponseDto>();
            CreateMap<CheckoutRequestDto, Checkout>();
        }
    }
}
