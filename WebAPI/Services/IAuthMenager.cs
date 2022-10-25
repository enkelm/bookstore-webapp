using API.Models.DTOs;

namespace WebAPI.Services
{
    public interface IAuthMenager
    {
        Task<bool> ValidateUser(LoginUserDTO userDTO);
        Task<string> CreateToken();
    }
}
