using krzysztofb.Authorization;
using krzysztofb.CustomExceptions;
using krzysztofb.Models;
using krzysztofb.Models.DTO;
using krzysztofb.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace krzysztofb.Services.Services
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
        private readonly IAuthorizationService _authorizationService;

        public UzytkownikService(WnioskiContext context, IPasswordHasher<Uzytkownik> passwordHasher, IAuthorizationService authorizationService)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _authorizationService = authorizationService;
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
        public UzytkownikDTO Delete(int id, ClaimsPrincipal user)
        {
            var userRecord = _context.Uzytkownik.Find(id);
            var authorizationResult = _authorizationService.AuthorizeAsync(user, userRecord, new OwnershipRequirement(userRecord.Id)).Result;
            if (!authorizationResult.Succeeded)
            {
                throw new DatabaseValidationException("Nie masz uprawnień do usunięcia tego wniosku");
            }
            if (userRecord == null)
            {
                throw new DatabaseValidationException("Użytkownik id: " + id + " nie istnieje");
            }
            _context.Uzytkownik.Remove(userRecord);

            return ModelConverter.ConvertToDTO(userRecord);
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