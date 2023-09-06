using krzysztofb.Models;
using krzysztofb.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace krzysztofb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WnioskiController : ControllerBase
    {
        private readonly WnioskiContext _context;
        private readonly WniosekTable _wniosekTable;

        public WnioskiController(WnioskiContext context, WniosekTable wniosekTable)
        {
            _context = context;
            _wniosekTable = wniosekTable;

        }

        // GET: api/Wnioseks
        [HttpGet]
        public async Task<List<WniosekDTO>> GetWnioski()
        {
            var wniosek = await Task.Run(() => _wniosekTable.Read());
            return wniosek;
        }

        // GET: api/Wnioseks/5
        [HttpGet("{id}")]
        public async Task<FileResult> GetWniosek(int id)
        {
            var file = new FileContentResult(_wniosekTable.Read(id).Plik, "application/pdf");
            return await Task.Run(() => file);


        }

        // PUT: api/Wnioseks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWniosek(int id, Wniosek wniosek)
        {
            if (id != wniosek.Id)
            {
                return BadRequest();
            }

            _context.Entry(wniosek).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WniosekExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Wnioseks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<WniosekDTO>> PostWniosek(IFormFile file)
        {

            return Task.FromResult(_wniosekTable.Create(file)).Result;
        }

        // DELETE: api/Wnioseks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWniosek(int id)
        {
            if (_context.Wniosek == null)
            {
                return NotFound();
            }
            var wniosek = await _context.Wniosek.FindAsync(id);
            if (wniosek == null)
            {
                return NotFound();
            }

            _context.Wniosek.Remove(wniosek);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool WniosekExists(int id)
        {
            return (_context.Wniosek?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
