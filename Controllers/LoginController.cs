using krzysztofb.Models.DTO;
using krzysztofb.Services.Services;
using Microsoft.AspNetCore.Mvc;

namespace krzysztofb.Controllers
{
    /// <summary>
    ///Controller obsługujący operacje na wnioskach
    /// </summary>
    [Route("api/")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly LoginServices _loginServices;

        public LoginController(LoginServices loginServices)
        {
            _loginServices = loginServices;
        }
        [HttpPost("login")]
        public IActionResult Login(UzytkownikLoginDTO uzytkownik)
        {
            var token = _loginServices.Login(uzytkownik);
            return Ok(token);
        }
    }
}