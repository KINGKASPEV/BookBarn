using BookBarn.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookBarn.Infrastructure.Context
{
    public class BookBarnDbContext : IdentityDbContext<AppUser>
    {
        public BookBarnDbContext(DbContextOptions<BookBarnDbContext> options) : base(options) { }
        public DbSet<Book> Books { get; set; }
        public DbSet<Cart> Carts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure one-to-many relationship between AppUser and Cart
            modelBuilder.Entity<Cart>()
                .HasOne(c => c.AppUser)
                .WithMany(u => u.Carts)
                .HasForeignKey(c => c.AppUserId);

            // Configure one-to-many relationship between Cart and Book
            modelBuilder.Entity<Book>()
                .HasOne(b => b.Cart)
                .WithMany(c => c.Books)
                .HasForeignKey(b => b.CartId);
        }
    }
}
