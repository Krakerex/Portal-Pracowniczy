using krzysztofb.Models;
using krzysztofb.Models.DTO;
using Microsoft.AspNetCore.Mvc;

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
            file.FileDownloadName = _wniosekTable.Read(id).Nazwa;
            return await Task.Run(() => file);


        }

        // PUT: api/Wnioseks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{idWniosek},{idKierownik}")]
        public async Task<ActionResult<WniosekDTO>> PutWniosek(int idWniosek, int idKierownik)
        {
            return Task.Run(() => _wniosekTable.Accept(idWniosek, idKierownik)).Result;
        }

        // POST: api/Wnioseks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<WniosekDTO>> PostWniosek(IFormFile file, int idSender)
        {
            var wniosek = new WniosekDTO
            {
                IdOsobyZglaszajacej = idSender,
                IdOsobyAkceptujacej = null
            };
            wniosek = _wniosekTable.SaveFile(wniosek, file);
            return Task.FromResult(_wniosekTable.Create(wniosek)).Result;
        }

        // DELETE: api/Wnioseks/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<WniosekDTO>> DeleteWniosek(int id)
        {
            return Task.Run(() => _wniosekTable.Delete(id)).Result;
        }

        private bool WniosekExists(int id)
        {
            return (_context.Wniosek?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
