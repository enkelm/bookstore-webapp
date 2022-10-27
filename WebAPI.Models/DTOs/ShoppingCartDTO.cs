using System.ComponentModel.DataAnnotations;

namespace API.Models.DTOs
{
    public class CreateShoppingCartDTO
    {
        public int ProductId { get; set; }

        [Range(1, 1000, ErrorMessage = "Please enter a value between 1 and 1000")]
        public int Count { get; set; }

        public int ApplicationUserId { get; set; }

    }
    public class ShoppingCartDTO : CreateShoppingCartDTO
    {
        public int Id { get; set; }
        public Product Product { get; set; }
        public ApiUser ApiUser { get; set; }
    }
}
