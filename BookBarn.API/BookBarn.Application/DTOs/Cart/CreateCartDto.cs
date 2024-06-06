namespace BookBarn.Application.DTOs.Cart
{
    public class CreateCartDto
    {
        public string AppUserId { get; set; }
        public ICollection<string> BookIds { get; set; }
    }
}
