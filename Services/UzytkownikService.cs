﻿using krzysztofb.Controllers;
using krzysztofb.CustomExceptions;
using krzysztofb.Models;
using krzysztofb.Models.DTO;
using krzysztofb.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace krzysztofb.Services
{
    /// <summary>
    /// Service służący do obsługi użytkowników
    /// </summary>
    public class UzytkownikService :
        IDatabaseRead<UzytkownikDTO>,
        IDatabaseUpdate<UzytkownikDTO>,
        IDatabaseDelete<UzytkownikDTO>,
        IDatabaseCreate<UzytkownikDTO>
    {
        private readonly WnioskiContext _context;


        public UzytkownikService(WnioskiContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Metoda służąca do walidacji i dodawania użytkownika do bazy danych
        /// </summary>
        /// <param name="obj">Objekt UżytkownikDTO do dodania</param>
        /// <returns>Dodany UżytkownikDTO</returns>
        /// <exception cref="BadHttpRequestException"></exception>
        /// <exception cref="DbUpdateException"></exception>
        public UzytkownikDTO Create(UzytkownikDTO obj)
        {
            switch (_context.Uzytkownik.Find(obj.IdPrzelozonego))
            {
                case null:
                    throw new DatabaseValidationException("Użytkownik podany za przełożonego nie istnieje");
                default:
                    if (_context.Uzytkownik.Find(obj.IdPrzelozonego).Role != 2)
                    {
                        int kierownikId = _context.Uzytkownik.Find(obj.IdPrzelozonego).Id;
                        throw new DatabaseValidationException("Użytkownik o id " + kierownikId + " nie jest kierownikiem");
                    }
                    else if (_context.Role.Find(obj.Role) == null)
                    {
                        throw new DatabaseValidationException("Podana rola nie istnieje");
                    }
                    else if (_context.Uzytkownik.FirstOrDefault(x => x.Email == obj.Email) != null)
                    {
                        throw new DatabaseValidationException("Podany email już istnieje w bazie danych");
                    }

                    break;
            }
            _context.Uzytkownik.Add(ModelConverter.ConvertToModel(obj));
            _context.SaveChanges();
            UzytkownicyController.StatusCode = 201;
            return obj;
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
            UzytkownicyController.StatusCode = 204;
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
            UzytkownicyController.StatusCode = 200;
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
            UzytkownicyController.StatusCode = 200;
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
                //check if obj contains any null fields excluding id
                if (obj.GetType().GetProperties().Any(x => x.GetValue(obj) == null && x.Name != "Id"))
                {
                    throw new DatabaseValidationException("Użytkownik o id: " + id + " nie istnieje");
                }
                else
                {
                    UzytkownicyController.StatusCode = 201;
                    return Create(obj);

                }

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
            UzytkownicyController.StatusCode = 200;
            return obj;

        }
    }
}
