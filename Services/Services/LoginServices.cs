using krzysztofb.Authentication;
using krzysztofb.Authorization;
using krzysztofb.CustomExceptions;
using krzysztofb.Models;
using krzysztofb.Models.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace krzysztofb.Services.Services
{
    public class LoginServices : ILoginServices
    {
        private readonly WnioskiContext _context;
        private readonly IPasswordHasher<Uzytkownik> _passwordHasher;
        private readonly AuthenticationSettings _authSettings;

        public LoginServices(WnioskiContext context, IPasswordHasher<Uzytkownik> passwordHasher, AuthenticationSettings authsettings)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _authSettings = authsettings;
        }

        public string Login(UzytkownikLoginDTO uzytkownik)
        {
            //select user first or default where email matches uzytkownik and include role
            var user = _context.Uzytkownik.Include(x => x.RoleNavigation).FirstOrDefault(x => x.Email == uzytkownik.email);
            if (user == null)
            {
                throw new DatabaseValidationException("Użytkownik o podanym emailu nie istnieje");
            }
            else if (_passwordHasher.VerifyHashedPassword(user, user.PasswordHash, uzytkownik.password) == PasswordVerificationResult.Failed)
            {
                throw new DatabaseValidationException("Podane hasło jest nieprawidłowe");
            }
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.Imie} {user.Nazwisko}"),
                new Claim(ClaimTypes.Role, user.RoleNavigation.Nazwa)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authSettings.JwtKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_authSettings.JwtExpireDays);

            var token = new JwtSecurityToken(_authSettings.JwtIssuer,
                _authSettings.JwtIssuer,
                claims,
                expires: expires,
                signingCredentials: cred);
            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(ClaimGenerator.GenerateToken(user));
        }
    }
}