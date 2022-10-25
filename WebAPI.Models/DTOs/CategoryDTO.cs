using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace API.Models.DTOs
{
    public class CategoryDTO
    {
        [Required]
        [StringLength(maximumLength: 100, ErrorMessage = "Category Name Is Too Long")]
        public string Name { get; set; }

        [DisplayName("Display Order")]
        [Range(1, 100, ErrorMessage = "Display Order must be between 1 and 100 only!!")]
        public int DisplayOrder { get; set; }

        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
    }

    public class CreateCategoryDTO : CategoryDTO
    {
        public int Id { get; set; }
        public IList<ProductDTO> ProductDTOs { get; set; }
    }
}
