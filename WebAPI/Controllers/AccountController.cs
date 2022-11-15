using API.Models;
using API.Models.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApiUser> _userManager;
        private readonly SignInManager<ApiUser> _signInManager;
        private readonly ILogger<AccountController> _logger;
        private readonly IMapper _mapper;
        private readonly IAuthMenager _authMenager;

        public AccountController(UserManager<ApiUser> userManager,
            SignInManager<ApiUser> signInManager,
            ILogger<AccountController> logger,
            IMapper mapper,
            IAuthMenager authMenager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _mapper = mapper;
            _authMenager = authMenager;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] ApiUserDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogInformation($"Registration Attempt for {userDTO.Email}");
                return BadRequest(ModelState);
            }
            try
            {
                var user = _mapper.Map<ApiUser>(userDTO);
                user.UserName = userDTO.Email;
                var result = await _userManager.CreateAsync(user, userDTO.Password);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }

                    return BadRequest("Something Went Wrong.");
                }

                await _userManager.AddToRolesAsync(user, userDTO.Roles);

                return Accepted();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something Went Wrong in the {nameof(Register)}");
                return Problem($"Something Went Wrong in the {nameof(Register)}", statusCode: 500);
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogInformation($"Login Attempt for {userDTO.Email}");
                return BadRequest(ModelState);
            }

            try
            {
                if (!await _authMenager.ValidateUser(userDTO))
                {
                    return Unauthorized();
                }

                var user = await _userManager.FindByEmailAsync(userDTO.Email);

                return Accepted(new { Token = await _authMenager.CreateToken(), Role = await _userManager.GetRolesAsync(user), UserId = await _userManager.GetUserIdAsync(user) });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something Went Wrong in the {nameof(Login)}");
                return Problem($"Something Went Wrong in the {nameof(Login)}", statusCode: 500);
            }
        }
    }
}
