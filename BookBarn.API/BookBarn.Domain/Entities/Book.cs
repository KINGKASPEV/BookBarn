using System.ComponentModel.DataAnnotations.Schema;

namespace BookBarn.Domain.Entities
{
    public class Book : BaseEntity
    {
        [ForeignKey("Cart")]
        public string CartId { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public string ImageUrl { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public string ISBN { get; set; }
        public string Author { get; set; }
        public int PublicationYear { get; set; }
        public Cart Cart { get; set; }
    }
}
