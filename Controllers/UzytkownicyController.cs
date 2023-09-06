using krzysztofb.Models;
using krzysztofb.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace krzysztofb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UzytkownicyController : ControllerBase
    {
        private readonly WnioskiContext _context;
        private readonly UserTable _userTable;

        public UzytkownicyController(WnioskiContext context, UserTable userTable)
        {
            _context = context;
            _userTable = userTable;
        }

        // GET: api/Uzytkownicy
        [HttpGet]
        public async Task<List<UzytkownikDTO>> GetUzytkownicy()
        {
            var uzytkownik = await Task.Run(() => _userTable.Read());
            return uzytkownik;
        }

        // GET: api/Uzytkowniks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UzytkownikDTO>> GetUzytkownik(int id)
        {
            if (_context.Uzytkownik == null)
            {
                return NotFound();
            }
            var uzytkownik = await Task.Run(() => _userTable.Read(id));

            if (uzytkownik == null)
            {
                return NotFound();
            }

            return uzytkownik;
        }

        // PUT: api/Uzytkowniks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<UzytkownikDTO>> PutUzytkownik(int id, UzytkownikDTO uzytkownik)
        {
            return await Task.Run(() => _userTable.Update(id, uzytkownik));
        }

        // POST: api/Uzytkowniks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UzytkownikDTO>> PostUzytkownik(UzytkownikDTO uzytkownik)
        {
            try
            {
                return await Task.Run(() => _userTable.Create(uzytkownik));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        // DELETE: api/Uzytkowniks/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<UzytkownikDTO>> DeleteUzytkownik(int id)
        {
            return await Task.Run(() => _userTable.Delete(id));
        }

        private bool UzytkownikExists(int id)
        {
            return (_context.Uzytkownik?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
