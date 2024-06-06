namespace BookBarn.Application.DTOs.Cart
{
    public class UpdateCartDto
    {
        public string Id { get; set; }
        public ICollection<string> BookIds { get; set; }
    }
}
