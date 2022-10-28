using API.Models.DTOs;
using AutoMapper;

namespace API.Models.Configurations
{
    public class MapperInitilizer : Profile
    {
        public MapperInitilizer()
        {
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<CoverType, CoverTypeDTO>().ReverseMap();
            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<ShoppingCart, ShoppingCartDTO>().ReverseMap();
            CreateMap<ApiUser, ApiUserDTO>().ReverseMap();

            CreateMap<Category, CreateCategoryDTO>().ReverseMap();
            CreateMap<CoverType, CreateCoverTypeDTO>().ReverseMap();
            CreateMap<Product, CreateProductDTO>().ReverseMap();
            CreateMap<ShoppingCart, CreateShoppingCartDTO>().ReverseMap();
        }
    }
}
