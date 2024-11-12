namespace CartMicroservice.Models
{
    public class CartItemDto
    {
        public string ProductId { get; set; }
        public string UserId { get; set; } // Passed from the frontend
        public int Quantity { get; set; }
    }

}
