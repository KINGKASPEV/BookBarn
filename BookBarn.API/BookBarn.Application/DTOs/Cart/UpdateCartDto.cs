﻿namespace BookBarn.Application.DTOs.Cart
{
    public class UpdateCartDto
    {
        public string CartId { get; set; }
        public ICollection<string> BookIds { get; set; }
    }
}
