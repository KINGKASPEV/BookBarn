namespace BookBarn.Application.DTOs.Checkout
{
    public class CheckoutRequestDto
    {
        public string PaymentMethod { get; set; } // "Web", "USSD", "Transfer"
    }
}
