using System.ComponentModel.DataAnnotations;

namespace API.Models.DTOs
{
    public class CoverTypeDTO
    {
        [Display(Name = "Cover Type")]
        [MaxLength(50)]
        public string Name { get; set; }
    }

    public class CreateCoverTypeDTO : CoverTypeDTO
    {
        public int Id { get; set; }
        public IList<ProductDTO> ProductDTOs { get; set; }
    }
}
