using krzysztofb.Models.DTO;
using krzysztofb.Services;
using Microsoft.AspNetCore.Mvc;

namespace krzysztofb.Controllers
{
    /// <summary>
    ///Controller obsługujący operacje na wnioskach
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class WnioskiController : ControllerBase
    {

        private readonly WniosekService _wniosekServices;

        public WnioskiController(WniosekService wniosekServices)
        {
            _wniosekServices = wniosekServices;
        }
        /// <summary>
        /// Metoda zwracająca listę wniosków WniosekDTO
        /// </summary>
        /// <returns>Lista obiektów WniosekDTO</returns>
        // GET: api/Wnioski
        [HttpGet]
        public List<WniosekDTO> GetWnioski()
        {
            var wniosek = _wniosekServices.Read();
            return wniosek;
        }
        /// <summary>
        /// Metoda pobierająca plik z wnioskiem na podstawie id
        /// </summary>
        /// <param name="id">id wniosku który chcemy pobrać</param>
        /// <returns>FileResult pliku pobranego z bazy danych</returns>
        // GET: api/Wnioski/5
        [HttpGet("{id}")]
        public FileResult GetWniosek(int id)
        {

            return _wniosekServices.DownloadFile(id);

        }
        /// <summary>
        /// Metoda służąca do akceptacji wniosków przez kierowników
        /// </summary>
        /// <param name="idWniosek">id wniosku który chcemy zaakceptować</param>
        /// <param name="idKierownik">id kierownika akceptującego wniosek</param>
        /// <returns>Zaakceptowany wniosek WniosekDTO</returns>
        // PUT: api/Wnioski/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{idWniosek},{idKierownik}")]
        public WniosekDTO PutWniosek(int idWniosek, int idKierownik)
        {
            Response.StatusCode = 201;
            return _wniosekServices.Accept(idWniosek, idKierownik);
        }
        /// <summary>
        /// Metoda słuząca do dodawania wniosków do bazy danych
        /// </summary>
        /// <param name="file">plik z wnioskiem</param>
        /// <param name="idSender">id osoby wysyłającej wniosek</param>
        /// <returns>Dodany wniosek WniosekDTO</returns>
        // POST: api/Wnioski
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{idSender}")]
        public WniosekDTO PostWniosek(IFormFile file, int idSender)
        {
            var wniosek = new WniosekDTO
            {
                IdOsobyZglaszajacej = idSender,
                IdOsobyAkceptujacej = null
            };
            wniosek = _wniosekServices.AddFile(wniosek, file);
            Response.StatusCode = 201;
            return _wniosekServices.Create(wniosek);
        }
        /// <summary>
        /// Metoda służąca do usuwania wniosków z bazy danych
        /// </summary>
        /// <param name="id">id wniosku do usunięcia</param>
        /// <returns>Usunięty wniosek WniosekDTO</returns>
        // DELETE: api/Wnioski/5
        [HttpDelete("{id}")]
        public WniosekDTO DeleteWniosek(int id)
        {
            Response.StatusCode = 200;
            return _wniosekServices.Delete(id);
        }
    }
}
