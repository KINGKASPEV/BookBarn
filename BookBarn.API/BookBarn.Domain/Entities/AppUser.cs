﻿using Microsoft.AspNetCore.Identity;

namespace BookBarn.Domain.Entities
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string PasswordResetToken { get; set; }
        public DateTime? ResetTokenExpires { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public DateTime DateModified { get; set; }
        public ICollection<Cart> Carts { get; set; }
        public ICollection<PurchaseHistory> PurchaseHistories { get; set; }
        public ICollection<Checkout> Checkouts { get; set; }
    }
}
