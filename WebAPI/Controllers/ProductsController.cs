using API.DataAccess.Repository.IRepository;
using API.Models;
using API.Models.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ProductsController> _logger;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _appEnvironment;

        public ProductsController(IUnitOfWork unitOfWork, ILogger<ProductsController> logger, IMapper mapper, IWebHostEnvironment appEnvironment)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _appEnvironment = appEnvironment;
        }

        // GET: api/Products
        [EnableCors]
        [HttpGet("GetAll")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            try
            {
                var categories = await _unitOfWork.Product.GetAllAsync();
                var results = _mapper.Map<IList<ProductDTO>>(categories);
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(GetProducts)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }
        }

        // GET: api/Products/5
        [EnableCors]
        [HttpGet("GetById/{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            try
            {
                var category = await _unitOfWork.Product.GetByIdAsync(u => u.Id == id);
                var result = _mapper.Map<ProductDTO>(category);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(GetProduct)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [EnableCors]
        [Authorize(Roles = "Administrator", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("Put/{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PutProduct(int id, [FromForm] CreateProductDTO productDTO)
        {
            if (!ModelState.IsValid || id < 1)
            {
                _logger.LogError($"Invalid POST attempt in {nameof(PutProduct)}");
                return BadRequest(ModelState);
            }
            try
            {
                var product = await _unitOfWork.Product.GetByIdAsync(u => u.Id == id);
                if (product == null)
                {
                    _logger.LogError($"Invalid POST attempt in {nameof(PutProduct)}");
                    return BadRequest("Submitted data is invalid.");
                }
                await UpdateImage(productDTO.Image, product.ImageUrl);
                _mapper.Map(productDTO, product);
                _unitOfWork.Product.Update(product);
                await _unitOfWork.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(PutProduct)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }
        }
        private async Task<IActionResult> UpdateImage(IFormFile file, string path)
        {
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return Ok();
        }

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [EnableCors]
        [Authorize(Roles = "Administrator", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("Post", Name = "PostProdut")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Product>> PostProduct([FromForm] CreateProductDTO productDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid POST attempt in {nameof(PostProduct)}");
                return BadRequest(ModelState);
            }
            try
            {
                string path = await UploadImage(productDTO.Image);
                productDTO.ImageUrl = path;
                var product = _mapper.Map<Product>(productDTO);
                _unitOfWork.Product.Add(product);
                await _unitOfWork.SaveChangesAsync();

                return CreatedAtRoute("PostProdut", new { id = product.Id }, product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(PostProduct)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }
        }

        private async Task<string> UploadImage(IFormFile file)
        {
            var special = Guid.NewGuid().ToString();
            var folderpath = _appEnvironment.WebRootPath + "\\Uploads\\ProductImages";
            var filename = special + "-" + file.FileName;

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), folderpath, filename);
            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return filePath;
        }

        // DELETE: api/Products/5
        [EnableCors]
        [Authorize(Roles = "Administrator", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("Delete/{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteProducts(int id)
        {
            if (id < 1)
            {
                _logger.LogError($"Invalid POST attempt in {nameof(DeleteProducts)}");
                return BadRequest(ModelState);
            }
            try
            {
                var product = await _unitOfWork.Product.GetByIdAsync(u => u.Id == id);

                if (product == null)
                {
                    _logger.LogError($"Invalid DELETE attempt in {nameof(DeleteProducts)}");
                    return BadRequest("Submitted data is invalid.");
                }

                _unitOfWork.Product.Remove(product);
                await _unitOfWork.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(DeleteProducts)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }
        }
    }
}
