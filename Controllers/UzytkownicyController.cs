using krzysztofb.Models;
using krzysztofb.Models.DTO;
using krzysztofb.Services;
using Microsoft.AspNetCore.Mvc;

namespace krzysztofb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UzytkownicyController : ControllerBase
    {
        private readonly UzytkownikService _uzytkownikService;

        public UzytkownicyController(WnioskiContext context, UzytkownikService uzytkownikService)
        {
            _uzytkownikService = uzytkownikService;
        }
        // GET: api/Uzytkownicy
        [HttpGet]
        public List<UzytkownikDTO> GetUzytkownicy()
        {
            return _uzytkownikService.Read();
        }
        // GET: api/Uzytkowniks/5
        [HttpGet("{id}")]
        public UzytkownikDTO GetUzytkownik(int id)
        {
            return _uzytkownikService.Read(id);
        }
        // PUT: api/Uzytkowniks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public UzytkownikDTO PutUzytkownik(int id, UzytkownikDTO uzytkownik)
        {
            Response.StatusCode = 201;
            return _uzytkownikService.Update(id, uzytkownik);
        }

        // POST: api/Uzytkowniks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public UzytkownikDTO PostUzytkownik(UzytkownikDTO uzytkownik)
        {
            Response.StatusCode = 201;

            return _uzytkownikService.Create(uzytkownik);
        }
        // DELETE: api/Uzytkowniks/5
        [HttpDelete("{id}")]
        public UzytkownikDTO DeleteUzytkownik(int id)
        {
            return _uzytkownikService.Delete(id);
        }
    }
}
