using API.DataAccess.Repository.IRepository;
using API.Models;
using API.Models.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CategoriesController> _logger;
        private readonly IMapper _mapper;

        public CategoriesController(IUnitOfWork unitOfWork, ILogger<CategoriesController> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        // GET: api/Categories
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<CoverType>>> GetCategories()
        {
            try
            {
                var categories = await _unitOfWork.Category.GetAllAsync();
                var results = _mapper.Map<IList<CategoryDTO>>(categories);
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(GetCategories)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }
        }

        // GET: api/Categories/5
        [HttpGet("{id}", Name = "GetCategory")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            try
            {
                var category = await _unitOfWork.Category.GetByIdAsync(u => u.Id == id);
                var result = _mapper.Map<CategoryDTO>(category);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(GetCategory)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }
        }

        // PUT: api/Categories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "Administrator")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PutCategory(int id, [FromBody] CreateCategoryDTO categoryDTO)
        {
            if (!ModelState.IsValid || id < 1)
            {
                _logger.LogError($"Invalid POST attempt in {nameof(PostCategory)}");
                return BadRequest(ModelState);
            }
            try
            {
                var category = await _unitOfWork.Category.GetByIdAsync(u => u.Id == id);
                if (category == null)
                {
                    _logger.LogError($"Invalid POST attempt in {nameof(PostCategory)}");
                    return BadRequest("Submitted data is invalid.");
                }
                _mapper.Map(categoryDTO, category);
                _unitOfWork.Category.Update(category);
                await _unitOfWork.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(PostCategory)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }
        }

        // POST: api/Categories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Category>> PostCategory([FromBody] CreateCategoryDTO categoryDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid POST attempt in {nameof(PostCategory)}");
                return BadRequest(ModelState);
            }
            try
            {
                var category = _mapper.Map<Category>(categoryDTO);
                _unitOfWork.Category.Add(category);
                await _unitOfWork.SaveChangesAsync();

                return CreatedAtRoute("GetCategory", new { id = category.Id }, category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(PostCategory)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }
        }

        // DELETE: api/Categories/5
        [Authorize(Roles = "Administrator")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            if (id < 1)
            {
                _logger.LogError($"Invalid POST attempt in {nameof(DeleteCategory)}");
                return BadRequest(ModelState);
            }
            try
            {
                var category = await _unitOfWork.Category.GetByIdAsync(u => u.Id == id);

                if (category == null)
                {
                    _logger.LogError($"Invalid DELETE attempt in {nameof(DeleteCategory)}");
                    return BadRequest("Submitted data is invalid.");
                }

                _unitOfWork.Category.Remove(category);
                await _unitOfWork.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(DeleteCategory)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }
        }
    }
}
