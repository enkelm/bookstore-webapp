namespace API.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int Count { get; set; }

        public int ApplicationUserId { get; set; }
        public ApiUser ApiUser { get; set; }
    }
}
