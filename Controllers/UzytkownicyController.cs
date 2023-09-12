using krzysztofb.Models;
using krzysztofb.Models.DTO;
using krzysztofb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace krzysztofb.Controllers
{
    /// <summary>
    /// Controller obsługujący operacje na użytkownikach
    /// </summary>
    [Route("api/uzytkownicy")]
    [ApiController]
    public class UzytkownicyController : ControllerBase
    {
        private readonly UzytkownikService _uzytkownikService;
        public static int StatusCode = 200;
        public UzytkownicyController(WnioskiContext context, UzytkownikService uzytkownikService)
        {
            _uzytkownikService = uzytkownikService;
        }
        /// <summary>
        /// Metoda zwracająca listę użytkowników UżytkownikDTO
        /// </summary>
        /// <returns>Lista obiektów UżytkownikDTO</returns>
        // GET: api/Uzytkownicy
        [HttpGet]
        public List<UzytkownikDTO> GetUzytkownicy()
        {
            return _uzytkownikService.Read();
        }
        /// <summary>
        /// Metoda zwracająca użytkownika na podstawie id
        /// </summary>
        /// <param name="id">Id użytkownika którego chcemy odczytać</param>
        /// <returns>Odczytany UżytkownikDTO</returns>
        // GET: api/Uzytkowniks/5
        [HttpGet("{id}")]
        public UzytkownikDTO GetUzytkownik(int id)
        {
            return _uzytkownikService.Read(id);
        }
        /// <summary>
        /// Metoda służąca do aktualizacji użytkownika
        /// </summary>
        /// <param name="id">Id użytkownika do zaaktualizowania</param>
        /// <param name="uzytkownik">Obiekt UżytkownikDTO z nowymi danymi</param>
        /// <returns>Zaaktualizowany UżytkownikDTO</returns>
        // PUT: api/Uzytkowniks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]

        public UzytkownikDTO PutUzytkownik(int id, [ValidateNever] UzytkownikDTO uzytkownik)
        {
            UzytkownikDTO uzytkownikDTO = _uzytkownikService.Update(id, uzytkownik);
            Response.StatusCode = StatusCode;
            return uzytkownikDTO;
        }
        /// <summary>
        /// Metoda służąca do dodawania użytkownika do bazy danych
        /// </summary>
        /// <param name="uzytkownik">Obiekt UżytkownikDTO z danymi do dodania</param>
        /// <returns>Dodany UżytkownikDTO</returns>
        // POST: api/Uzytkowniks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public UzytkownikDTO PostUzytkownik(UzytkownikDTO uzytkownik)
        {

            Response.StatusCode = StatusCode;
            return _uzytkownikService.Create(uzytkownik);

        }
        /// <summary>
        /// Metoda służąca do usuwania użytkownika z bazy danych
        /// </summary>
        /// <param name="id">Id użytkownika do usunięcia</param>
        /// <returns>Usunięty UżytkownikDTO</returns>
        // DELETE: api/Uzytkowniks/5
        [HttpDelete("{id}")]
        public UzytkownikDTO DeleteUzytkownik(int id)
        {
            return _uzytkownikService.Delete(id);
        }
    }
}
