﻿using krzysztofb.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace krzysztofb.Controllers
{
    /// <summary>
    ///Controller obsługujący operacje na wnioskach
    /// </summary>
    [Route("api/wnioski")]
    [ApiController]
    [ResponseCache(Duration = 1200)]
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
        public IActionResult GetWnioski()
        {
            var wniosek = _wniosekServices.Read();
            return Ok(wniosek);
        }

        /// <summary>
        /// Metoda pobierająca plik z wnioskiem na podstawie id
        /// </summary>
        /// <param name="id">id wniosku który chcemy pobrać</param>
        /// <returns>FileResult pliku pobranego z bazy danych</returns>
        // GET: api/Wnioski/5
        [HttpGet("download/{id}")]
        public FileResult DownloadWniosek(int id)
        {
            return _wniosekServices.DownloadFile(id);
        }

        [HttpGet("{id}")]
        public IActionResult GetWniosek(int id)
        {
            return Ok(_wniosekServices.Read(id));
        }

        /// <summary>
        /// Metoda służąca do akceptacji wniosków przez kierowników
        /// </summary>
        /// <param name="idWniosek">id wniosku który chcemy zaakceptować</param>
        /// <param name="idKierownik">id kierownika akceptującego wniosek</param>
        /// <returns>Zaakceptowany wniosek WniosekDTO</returns>
        // PUT: api/Wnioski/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{idWniosek}")]
        [Authorize(Roles = "Kierownik")]
        public async Task<IActionResult> PutWniosek(int idWniosek)
        {
            int id = Int32.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            _wniosekServices.Accept(idWniosek, id);
            return await Task.FromResult(Ok());
        }

        /// <summary>
        /// Metoda słuząca do dodawania wniosków do bazy danych
        /// </summary>
        /// <param name="file">plik z wnioskiem</param>
        /// <param name="idSender">id osoby wysyłającej wniosek</param>
        /// <returns>Dodany wniosek WniosekDTO</returns>
        // POST: api/Wnioski
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public IActionResult PostWniosek(IFormFile file)
        {
            return Ok(_wniosekServices.Create(file));
        }

        /// <summary>
        /// Metoda służąca do usuwania wniosków z bazy danych
        /// </summary>
        /// <param name="id">id wniosku do usunięcia</param>
        /// <returns>Usunięty wniosek WniosekDTO</returns>
        // DELETE: api/Wnioski/5
        [HttpDelete("{id}")]
        public IActionResult DeleteWniosek(int id)
        {
            return Ok(_wniosekServices.Delete(id, User));
        }
    }
}