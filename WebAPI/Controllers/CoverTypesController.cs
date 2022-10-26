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
    public class CoverTypesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CoverTypesController> _logger;
        private readonly IMapper _mapper;

        public CoverTypesController(IUnitOfWork unitOfWork, ILogger<CoverTypesController> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        // GET: api/CoverTypes
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<CoverType>>> GetCoverTypes()
        {
            try
            {
                var categories = await _unitOfWork.CoverType.GetAllAsync();
                var results = _mapper.Map<IList<CoverTypeDTO>>(categories);
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(GetCoverTypes)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }
        }

        // GET: api/CoverTypes/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CoverType>> GetCoverType(int id)
        {
            try
            {
                var category = await _unitOfWork.CoverType.GetByIdAsync(u => u.Id == id);
                var result = _mapper.Map<CoverTypeDTO>(category);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(GetCoverType)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }
        }

        // PUT: api/CoverTypes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "Administrator")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PutCoverType(int id, [FromBody] CreateCoverTypeDTO coverTypeDTO)
        {
            if (!ModelState.IsValid || id < 1)
            {
                _logger.LogError($"Invalid POST attempt in {nameof(PutCoverType)}");
                return BadRequest(ModelState);
            }
            try
            {
                var coverType = await _unitOfWork.CoverType.GetByIdAsync(u => u.Id == id);
                if (coverType == null)
                {
                    _logger.LogError($"Invalid POST attempt in {nameof(PutCoverType)}");
                    return BadRequest("Submitted data is invalid.");
                }
                _mapper.Map(coverTypeDTO, coverType);
                _unitOfWork.CoverType.Update(coverType);
                await _unitOfWork.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(PutCoverType)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }
        }

        // POST: api/CoverTypes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CoverType>> PostCoverType([FromBody] CreateCoverTypeDTO coverTypeDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid POST attempt in {nameof(PostCoverType)}");
                return BadRequest(ModelState);
            }
            try
            {
                var coverType = _mapper.Map<CoverType>(coverTypeDTO);
                _unitOfWork.CoverType.Add(coverType);
                await _unitOfWork.SaveChangesAsync();

                return CreatedAtRoute("GetCategory", new { id = coverType.Id }, coverType);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(PostCoverType)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }
        }

        // DELETE: api/CoverTypes/5
        [Authorize(Roles = "Administrator")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteCoverTypes(int id)
        {
            if (id < 1)
            {
                _logger.LogError($"Invalid POST attempt in {nameof(DeleteCoverTypes)}");
                return BadRequest(ModelState);
            }
            try
            {
                var coverType = await _unitOfWork.CoverType.GetByIdAsync(u => u.Id == id);

                if (coverType == null)
                {
                    _logger.LogError($"Invalid DELETE attempt in {nameof(DeleteCoverTypes)}");
                    return BadRequest("Submitted data is invalid.");
                }

                _unitOfWork.CoverType.Remove(coverType);
                await _unitOfWork.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went Wrong in the {nameof(DeleteCoverTypes)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later.");
            }
        }
    }
}
