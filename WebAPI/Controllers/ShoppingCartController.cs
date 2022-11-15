using API.DataAccess.Repository.IRepository;
using API.Models;
using API.Models.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApiUser> _userManager;
        private readonly ILogger<ShoppingCartController> _logger;
        private readonly IMapper _mapper;

        public ShoppingCartController(IUnitOfWork unitOfWork, UserManager<ApiUser> userMenager, ILogger<ShoppingCartController> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userManager = userMenager;
            _logger = logger;
            _mapper = mapper;
        }

        // GET: api/ShoppingCart
        [EnableCors]
        [HttpGet("GetAll")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ShoppingCart>>> GetShoppingCarts()
        {
            try
            {
                var categories = await _unitOfWork.ShoppingCart.GetAllAsync();
                var results = _mapper.Map<IList<ShoppingCartDTO>>(categories);
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(GetShoppingCarts)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }
        }

        // GET: api/ShoppingCart/5
        [EnableCors]
        [HttpGet("GetById/{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ShoppingCart>> GetShoppingCart(int id)
        {
            try
            {
                var category = await _unitOfWork.ShoppingCart.GetByIdAsync(u => u.Id == id);
                var result = _mapper.Map<ShoppingCartDTO>(category);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(GetShoppingCart)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }
        }

        //GET: api/ShoppingCart/GetByUser
        [EnableCors]
        [HttpGet("GetByUser/{userId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ShoppingCart>> GetShoppingCartByUser(string userId)
        {
            try
            {
                var cartHistory = await _unitOfWork.ShoppingCart.GetByUser(userId);
                var result = _mapper.Map<IList<ShoppingCartDTO>>(cartHistory);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(GetShoppingCartByUser)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }
        }

        // PUT: api/ShoppingCart/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [EnableCors]
        [HttpPut("Put/{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PutShoppingCart(int id, [FromBody] CreateShoppingCartDTO shoppingCartDTO)
        {
            if (!ModelState.IsValid || id < 1)
            {
                _logger.LogError($"Invalid POST attempt in {nameof(PutShoppingCart)}");
                return BadRequest(ModelState);
            }
            try
            {
                var shoppingCart = await _unitOfWork.ShoppingCart.GetByIdAsync(u => u.Id == id);
                if (shoppingCart == null)
                {
                    _logger.LogError($"Invalid POST attempt in {nameof(PutShoppingCart)}");
                    return BadRequest("Submitted data is invalid.");
                }
                _mapper.Map(shoppingCartDTO, shoppingCart);
                _unitOfWork.ShoppingCart.Update(shoppingCart);
                await _unitOfWork.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(PutShoppingCart)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }
        }

        // POST: api/ShoppingCart
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [EnableCors]
        [HttpPost("Post", Name = "PostShoppingCart")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ShoppingCart>> PostShoppingCart([FromBody] CreateShoppingCartDTO shoppingCartDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid POST attempt in {nameof(PostShoppingCart)}");
                return BadRequest(ModelState);
            }
            try
            {
                var shoppingCart = _mapper.Map<ShoppingCart>(shoppingCartDTO);
                _unitOfWork.ShoppingCart.Add(shoppingCart);
                await _unitOfWork.SaveChangesAsync();

                return CreatedAtRoute("PostShoppingCart", new { id = shoppingCart.Id }, shoppingCart);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(PostShoppingCart)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }
        }

        // DELETE: api/ShoppingCart/5
        [EnableCors]
        [HttpDelete("Delete/{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteShoppingCart(int id)
        {
            if (id < 1)
            {
                _logger.LogError($"Invalid POST attempt in {nameof(DeleteShoppingCart)}");
                return BadRequest(ModelState);
            }
            try
            {
                var shoppingCart = await _unitOfWork.ShoppingCart.GetByIdAsync(u => u.Id == id);

                if (shoppingCart == null)
                {
                    _logger.LogError($"Invalid DELETE attempt in {nameof(DeleteShoppingCart)}");
                    return BadRequest("Submitted data is invalid.");
                }

                _unitOfWork.ShoppingCart.Remove(shoppingCart);
                await _unitOfWork.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(DeleteShoppingCart)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }
        }
    }
}
