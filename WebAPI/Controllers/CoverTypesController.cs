using API.DataAccess.Repository.IRepository;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoverTypesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CoverTypesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/CoverTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CoverType>>> GetCoverTypes()
        {
            return Ok(await _unitOfWork.CoverType.GetAllAsync());
        }

        // GET: api/CoverTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CoverType>> GetCoverType(int id)
        {
            return Ok(await _unitOfWork.CoverType.GetByIdAsync(u => u.Id == id));
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
