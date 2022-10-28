using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace API.Models.DTOs
{
    public class CreateProductDTO
    {

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string ISBN { get; set; }

        [Required]
        public string Author { get; set; }

        [Required]
        [Range(1, 1000)]
        public double ListPrice { get; set; }

        [Required]
        [Range(1, 1000)]
        public double Price { get; set; }

        [Required]
        [Range(1, 1000)]
        public double Price50 { get; set; }

        [Required]
        [Range(1, 1000)]
        public double Price100 { get; set; }

        public string? ImageUrl { get; set; }

        public IFormFile Image { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public int CoverTypeId { get; set; }
    }

    public class ProductDTO : CreateProductDTO
    {
        public int Id { get; set; }
        public CategoryDTO Category { get; set; }
        public CoverTypeDTO CoverType { get; set; }

    }
}
