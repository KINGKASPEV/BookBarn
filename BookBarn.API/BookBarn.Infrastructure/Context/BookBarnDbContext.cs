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
        public DbSet<PurchaseHistory> PurchaseHistories { get; set; }
        public DbSet<Checkout> Checkouts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // One-to-many relationship between AppUser and Cart
            modelBuilder.Entity<Cart>()
                .HasOne(c => c.AppUser)
                .WithMany(u => u.Carts)
                .HasForeignKey(c => c.AppUserId);

            // One-to-many relationship between Cart and Book
            modelBuilder.Entity<Book>()
                .HasOne(b => b.Cart)
                .WithMany(c => c.Books)
                .HasForeignKey(b => b.CartId);

            // One-to-many relationship between AppUser and PurchaseHistory
            modelBuilder.Entity<PurchaseHistory>()
                .HasOne(ph => ph.AppUser)
                .WithMany(u => u.PurchaseHistories)
                .HasForeignKey(ph => ph.AppUserId);

            // One-to-many relationship between Cart and Checkout
            modelBuilder.Entity<Checkout>()
                .HasOne(co => co.Cart)
                .WithMany(c => c.Checkouts)
                .HasForeignKey(co => co.CartId);

            // One-to-many relationship between AppUser and Checkout
            modelBuilder.Entity<Checkout>()
                .HasOne(co => co.AppUser)
                .WithMany(u => u.Checkouts)
                .HasForeignKey(co => co.AppUserId);

            modelBuilder.Entity<PurchaseHistory>()
                .Property(ph => ph.TotalAmount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Book>()
                .Property(ph => ph.Price)
                .HasColumnType("decimal(18,2)");
        }
    }
}
