﻿using krzysztofb.Models.DTO;
using krzysztofb.Services;
using Microsoft.AspNetCore.Mvc;

namespace krzysztofb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WnioskiController : ControllerBase
    {

        private readonly WniosekService _wniosekServices;

        public WnioskiController(WniosekService wniosekServices)
        {
            _wniosekServices = wniosekServices;
        }

        // GET: api/Wnioski
        [HttpGet]
        public List<WniosekDTO> GetWnioski()
        {
            var wniosek = _wniosekServices.Read();
            return wniosek;
        }

        // GET: api/Wnioski/5
        [HttpGet("{id}")]
        public FileResult GetWniosek(int id)
        {

            return _wniosekServices.DownloadFile(id);

        }

        // PUT: api/Wnioski/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{idWniosek},{idKierownik}")]
        public WniosekDTO PutWniosek(int idWniosek, int idKierownik)
        {
            Response.StatusCode = 201;
            return _wniosekServices.Accept(idWniosek, idKierownik);
        }

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

        // DELETE: api/Wnioski/5
        [HttpDelete("{id}")]
        public WniosekDTO DeleteWniosek(int id)
        {
            Response.StatusCode = 200;
            return _wniosekServices.Delete(id);
        }
    }
}
