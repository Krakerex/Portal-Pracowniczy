using krzysztofb.Authentication;
using krzysztofb.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace krzysztofb.Authorization
{
    public class ClaimGenerator
    {
        //inject authSettings
        private static AuthenticationSettings _authSettings;

        public ClaimGenerator(AuthenticationSettings authSettings)
        {
            _authSettings = authSettings;
        }

        public static JwtSecurityToken GenerateToken(Uzytkownik user)
        {
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
            return token;
        }
    }
}