using BookBarn.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookBarn.Infrastructure.Context
{
    public class BookBarnDbContext : IdentityDbContext<AppUser>
    {
        public BookBarnDbContext(DbContextOptions<BookBarnDbContext> options) : base(options) { }
        public DbSet<Book> Books { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
