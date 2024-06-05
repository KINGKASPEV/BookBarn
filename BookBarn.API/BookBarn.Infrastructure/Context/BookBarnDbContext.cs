using BookBarn.Domain.Entities;
using System.Collections.Generic;
using System.Reflection.Emit;

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
