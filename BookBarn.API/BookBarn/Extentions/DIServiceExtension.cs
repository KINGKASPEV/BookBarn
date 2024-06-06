using BookBarn.Application.Services.Implementations;
using BookBarn.Application.Services.Interfaces;
using BookBarn.Domain.Entities;
using BookBarn.Infrastructure.Context;
using BookBarn.Infrastructure.Repositories.Implementations;
using BookBarn.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookBarn.Extentions
{
    public static class DIServiceExtension
    {
        public static void AddDependencies(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<RoleManager<IdentityRole>>();
            services.AddDbContext<BookBarnDbContext>(options =>
            options.UseSqlServer(config.GetConnectionString("DefaultConnection")));
            services.AddIdentity<AppUser, IdentityRole>()
            .AddEntityFrameworkStores<BookBarnDbContext>()
            .AddDefaultTokenProviders();
        }
    }
}
