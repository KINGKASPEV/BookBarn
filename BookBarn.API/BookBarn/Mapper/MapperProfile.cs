using AutoMapper;
using BookBarn.Application.DTOs.Auth;
using BookBarn.Application.DTOs.Book;
using BookBarn.Application.DTOs.Cart;
using BookBarn.Application.DTOs.Checkout;
using BookBarn.Application.DTOs.PurchaseHistory;
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
            CreateMap<Cart, CartDto>()
            .ForMember(dest => dest.Books, opt => opt.MapFrom(src => src.Books));
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
            CreateMap<Checkout, CheckoutResponseDto>()
            .ForMember(dest => dest.AppUserId, opt => opt.MapFrom(src => src.AppUserId));
            CreateMap<PurchaseHistory, PurchaseHistoryDto>()
            .ForMember(dest => dest.AppUserId, opt => opt.MapFrom(src => src.AppUserId))
            .ForMember(dest => dest.CheckoutId, opt => opt.MapFrom(src => src.CheckoutId))
            .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount))
            .ForMember(dest => dest.PurchaseDate, opt => opt.MapFrom(src => src.PurchaseDate))
            .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.Checkout.PaymentMethod))
            .ForMember(dest => dest.Books, opt => opt.MapFrom(src => src.Checkout.Cart.Books));
        }
    }
}
