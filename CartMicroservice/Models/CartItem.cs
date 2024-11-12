namespace CartMicroservice.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public string ProductId { get; set; }
        public string UserId { get; set; } // Unique identifier for the user
        public int Quantity { get; set; }
    }

}
