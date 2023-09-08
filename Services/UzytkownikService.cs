using krzysztofb.Interfaces;
using krzysztofb.Models;
using krzysztofb.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace krzysztofb.Services
{
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
        public UzytkownikDTO Create(UzytkownikDTO obj)
        {
            if (_context.Uzytkownik.Find(obj.IdPrzelozonego) == null)
            {
                throw new BadHttpRequestException("User with given idPrzelozony not found");
            }
            else if (_context.Role.Find(obj.Role) == null)
            {
                throw new BadHttpRequestException("Role not found");
            }
            else if (_context.Uzytkownik.FirstOrDefault(x => x.Email == obj.Email) != null)
            {
                throw new DbUpdateException("Email is not unique");
            }
            _context.Uzytkownik.Add(ModelConverter.ConvertToModel(obj));
            _context.SaveChanges();
            return obj;
        }

        public UzytkownikDTO Delete(int id)
        {
            var user = _context.Uzytkownik.Find(id);
            if (user == null)
            {
                throw new BadHttpRequestException("User not found");
            }
            _context.Uzytkownik.Remove(user);
            return ModelConverter.ConvertToDTO(user);
        }
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
        public UzytkownikDTO Read(int id)
        {
            if (_context.Uzytkownik.Find(id) == null)
            {
                throw new NullReferenceException("User not found");
            }
            var user = _context.Uzytkownik
                .Include(x => x.IdPrzelozonegoNavigation)
                .Include(x => x.RoleNavigation)
                .FirstOrDefault(x => x.Id == id);
            return ModelConverter.ConvertToDTO(user);
        }
        public UzytkownikDTO Update(int id, UzytkownikDTO obj)
        {
            if (_context.Uzytkownik.Find(id) == null)
            {
                throw new BadHttpRequestException("User not found");
            }
            else if (_context.Uzytkownik.Find(obj.IdPrzelozonego) == null)
            {
                throw new BadHttpRequestException("User with given idPrzelozony not found");
            }
            else if (_context.Role.Find(obj.Role) == null)
            {
                throw new BadHttpRequestException("Role not found");
            }
            else if (_context.Uzytkownik.FirstOrDefault(x => x.Email == obj.Email) != null)
            {
                throw new DbUpdateException("Email is not unique");
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
