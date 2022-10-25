using System.ComponentModel.DataAnnotations;

namespace API.Models.DTOs
{
    public class ApiUserDTO : LoginUserDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        public ICollection<string> Roles { get; set; }
    }

    public class LoginUserDTO
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [StringLength(15, ErrorMessage = "Your Password is limited from {2} to {1} characters", MinimumLength = 8)]
        public string Password { get; set; }
    }
}
