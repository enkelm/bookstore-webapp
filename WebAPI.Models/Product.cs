using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string ISBN { get; set; }

        public string Author { get; set; }

        public double ListPrice { get; set; }

        public double Price { get; set; }

        public double Price50 { get; set; }

        public double Price100 { get; set; }

        public string? ImageUrl { get; set; }
        [NotMapped]
        public IFormFile Image { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public int CoverTypeId { get; set; }
        public CoverType CoverType { get; set; }
    }
}
