using API.DataAccess.Repository.IRepository;
using API.Models;
using API.Models.DTOs;
using AutoMapper;
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
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCoverType(int id, CoverType covertype)
        {
            covertype.Id = id;
            _unitOfWork.CoverType.Update(covertype);
            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/CoverTypes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CoverType>> PostCoverType(CoverType covertype)
        {
            _unitOfWork.CoverType.Add(covertype);
            await _unitOfWork.SaveChangesAsync();

            return CreatedAtAction("GetCoverType", new { id = covertype.Id }, covertype);
        }

        // DELETE: api/CoverTypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCoverTypes(int id)
        {
            var covertype = await _unitOfWork.CoverType.GetByIdAsync(u => u.Id == id);
            if (covertype == null)
            {
                return NotFound();
            }

            _unitOfWork.CoverType.Remove(covertype);
            await _unitOfWork.SaveChangesAsync();

            return NoContent();
        }
    }
}
