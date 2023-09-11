using krzysztofb.Interfaces;
using krzysztofb.Models;
using krzysztofb.Models.DTO;
using Microsoft.EntityFrameworkCore;

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
            if (_context.Uzytkownik.Find(obj.IdPrzelozonego) == null)
            {
                throw new BadHttpRequestException("Użytkownik podany za przełożonego nie istnieje");
            }
            else if (_context.Uzytkownik.Find(obj.IdPrzelozonego).Role != 2)
            {
                throw new BadHttpRequestException("Użytkownik podany za przełożonego nie jest kierownikiem");
            }
            else if (_context.Role.Find(obj.Role) == null)
            {
                throw new BadHttpRequestException("Podana rola nie istnieje");
            }
            else if (_context.Uzytkownik.FirstOrDefault(x => x.Email == obj.Email) != null)
            {
                throw new DbUpdateException("Podany email już istnieje w bazie danych");
            }
            _context.Uzytkownik.Add(ModelConverter.ConvertToModel(obj));
            _context.SaveChanges();
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
                throw new BadHttpRequestException("Użytkownik o podanym id nie istnieje");
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
                throw new NullReferenceException("Użytkownik o podanym id nie istnieje");
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
                throw new BadHttpRequestException("Użytkownik o podanym id nie istnieje");
            }
            else if (_context.Uzytkownik.Find(obj.IdPrzelozonego) == null)
            {
                throw new BadHttpRequestException("Użytkownik podany za przełożonego nie istnieje");
            }
            else if (_context.Uzytkownik.Find(obj.IdPrzelozonego).Role != 2)
            {
                throw new BadHttpRequestException("Użytkownik podany za przełożonego nie jest kierownikiem");
            }
            else if (_context.Role.Find(obj.Role) == null)
            {
                throw new BadHttpRequestException("Podana rola nie istnieje");
            }
            else if (_context.Uzytkownik.FirstOrDefault(x => x.Email == obj.Email) != null)
            {
                throw new DbUpdateException("Podany email już istnieje w bazie danych");
            }
            var user = _context.Uzytkownik.Find(id);

            foreach (var propetryEntry in _context.Entry(user).Properties)
            {
                if (propetryEntry.Metadata.Name != "Id")
                {
                    propetryEntry.CurrentValue = obj.GetType().GetProperty(propetryEntry.Metadata.Name).GetValue(obj);
                }
            }
            _context.SaveChanges();
            return obj;

        }
    }
}
