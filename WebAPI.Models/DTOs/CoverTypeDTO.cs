using System.ComponentModel.DataAnnotations;

namespace API.Models.DTOs
{
    public class CreateCoverTypeDTO
    {
        [Display(Name = "Cover Type")]
        [MaxLength(50)]
        public string Name { get; set; }
    }

    public class CoverTypeDTO : CreateCoverTypeDTO
    {
        public int Id { get; set; }
        public IList<ProductDTO> ProductDTOs { get; set; }
    }
}
