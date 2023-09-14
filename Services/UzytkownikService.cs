using krzysztofb.Authentication;
using krzysztofb.CustomExceptions;
using krzysztofb.Models;
using krzysztofb.Models.DTO;
using krzysztofb.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace krzysztofb.Services
{
    /// <summary>
    /// Service służący do obsługi użytkowników
    /// </summary>
    public class UzytkownikService :
        IDatabaseRead<UzytkownikDTO>,
        IDatabaseUpdate<UzytkownikDTO>,
        IDatabaseDelete<UzytkownikDTO>,
        IDatabaseCreate<UzytkownikCreateDTO, UzytkownikDTO>
    {
        private readonly WnioskiContext _context;
        private readonly IPasswordHasher<Uzytkownik> _passwordHasher;
        private readonly AuthenticationSettings _authSettings;

        public UzytkownikService(WnioskiContext context, IPasswordHasher<Uzytkownik> passwordHasher, AuthenticationSettings authsettings)
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
            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// Metoda służąca do walidacji i dodawania użytkownika do bazy danych
        /// </summary>
        /// <param name="obj">Objekt UżytkownikDTO do dodania</param>
        /// <returns>Dodany UżytkownikDTO</returns>
        /// <exception cref="BadHttpRequestException"></exception>
        /// <exception cref="DbUpdateException"></exception>
        public UzytkownikDTO Create(UzytkownikCreateDTO obj)
        {
            if (_context.Uzytkownik.Find(obj.IdPrzelozonego) == null)
            {
                throw new DatabaseValidationException("Użytkownik podany za przełożonego nie istnieje");
            }
            else if (_context.Uzytkownik.Find(obj.IdPrzelozonego).Role != 2)
            {
                throw new DatabaseValidationException("Użytkownik podany za przełożonego nie istnieje");
            }
            else if (_context.Role.Find(obj.Role) == null)
            {
                throw new DatabaseValidationException("Podana rola nie istnieje");
            }
            else if (_context.Uzytkownik.FirstOrDefault(x => x.Email == obj.Email) != null)
            {
                throw new DatabaseValidationException("Podany email już istnieje w bazie danych");
            }
            var password = _passwordHasher.HashPassword(ModelConverter.ConvertToModel(obj), obj.Password);
            obj.Password = password;
            var created = _context.Uzytkownik.Add(ModelConverter.ConvertToModel(obj));
            _context.SaveChanges();

            return ModelConverter.ConvertToDTO(obj, created.Entity.Id);
        }

        /// <summary>
        /// Metoda służąca do walidacji i usuwania użytkownika z bazy danych
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Usunięty UżytkownikDTO</returns>
        /// <exception cref="BadHttpRequestException"></exception>
        public UzytkownikDTO Delete(int id)
        {
            var user = _context.Uzytkownik.Find(id);
            if (user == null)
            {
                throw new DatabaseValidationException("Użytkownik id: " + id + " nie istnieje");
            }
            _context.Uzytkownik.Remove(user);

            return ModelConverter.ConvertToDTO(user);
        }

        /// <summary>
        /// Metoda służąca do wczytania użytkowników z bazy danych do listy
        /// </summary>
        /// <returns>Lista UżytkownikDTO</returns>
        public List<UzytkownikDTO> Read()
        {
            //return all users converted to DTO
            var user = _context.Uzytkownik
               .Include(x => x.IdPrzelozonegoNavigation)
               .Include(x => x.RoleNavigation)
               .Select(x => ModelConverter.ConvertToDTO(x));

            return user
               .ToList();
        }

        /// <summary>
        /// Metoda służąca do walidacji i odczytania użytkownika o podanym id
        /// </summary>
        /// <param name="id">Id użytkownika do odczytania</param>
        /// <returns>Odczytany UżytkownikDTO</returns>
        /// <exception cref="NullReferenceException"></exception>
        public UzytkownikDTO Read(int id)
        {
            if (_context.Uzytkownik.Find(id) == null)
            {
                throw new DatabaseValidationException("Użytkownik id: " + id + " nie istnieje");
            }
            var user = _context.Uzytkownik
                .Include(x => x.IdPrzelozonegoNavigation)
                .Include(x => x.RoleNavigation)
                .FirstOrDefault(x => x.Id == id);

            return ModelConverter.ConvertToDTO(user);
        }

        /// <summary>
        /// Metoda służąca do walidacji i aktualizacji użytkownika w bazie danych
        /// </summary>
        /// <param name="id">Id użytkownika do zaaktualizowania</param>
        /// <param name="obj">UżytkownikDTO z nowymi danymi</param>
        /// <returns>Zaaktualizowany UżytkownikDTO</returns>
        /// <exception cref="BadHttpRequestException"></exception>
        /// <exception cref="DbUpdateException"></exception>
        public UzytkownikDTO Update(int id, UzytkownikDTO obj)
        {
            if (_context.Uzytkownik.Find(id) == null)
            {
                throw new DatabaseValidationException("Użytkownik o id: " + id + " nie istnieje");
            }
            else if (_context.Uzytkownik.Find(obj.IdPrzelozonego) == null && obj.IdPrzelozonego.HasValue)
            {
                throw new DatabaseValidationException("Użytkownik o id: " + obj.IdPrzelozonego + " podany za przełożonego nie istnieje");
            }
            else if (obj.IdPrzelozonego.HasValue)
            {
                if (_context.Uzytkownik.Find(obj.IdPrzelozonego).Role != 2)
                {
                    throw new DatabaseValidationException("Użytkownik o id: " + obj.IdPrzelozonego + " podany za przełożonego nie jest kierownikiem");
                }
            }
            else if (_context.Role.Find(obj.Role) == null && obj.Role.HasValue)
            {
                throw new DatabaseValidationException("Podana rola o id: " + obj.Role + " nie istnieje");
            }
            else if (_context.Uzytkownik.FirstOrDefault(x => x.Email == obj.Email) != null && !obj.Email.IsNullOrEmpty())
            {
                throw new DatabaseValidationException("Podany email: " + obj.Email + " już istnieje w bazie danych");
            }
            var user = _context.Uzytkownik.Find(id);

            foreach (var propetryEntry in _context.Entry(user).Properties)
            {
                if (propetryEntry.Metadata.Name != "Id")
                {
                    if (obj.GetType().GetProperty(propetryEntry.Metadata.Name).GetValue(obj) != null)
                        propetryEntry.CurrentValue = obj.GetType().GetProperty(propetryEntry.Metadata.Name).GetValue(obj);
                }
            }
            _context.SaveChanges();

            return obj;
        }
    }
}