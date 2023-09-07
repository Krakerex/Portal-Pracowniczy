using krzysztofb.Interfaces;
using krzysztofb.Models;
using krzysztofb.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace krzysztofb
{
    public class UserTable :
        IDatabaseRead<UzytkownikDTO>,
        IDatabaseUpdate<UzytkownikDTO>,
        IDatabaseDelete<UzytkownikDTO>,
        IModelConverter<Uzytkownik, UzytkownikDTO>
    {
        private readonly WnioskiContext _context;
        public UserTable(WnioskiContext context)
        {
            _context = context;
        }

        public static UzytkownikDTO ConvertToDTO(Uzytkownik uzytkownik)
        {
            return new UzytkownikDTO
                (
                uzytkownik.Id,
                uzytkownik.Imie,
                uzytkownik.Nazwisko,
                uzytkownik.Email,
                uzytkownik.Role,
                uzytkownik.IdPrzelozonego
                );
        }

        public static Uzytkownik ConvertToModel(UzytkownikDTO uzytkownik)
        {
            return new Uzytkownik
            {
                Id = uzytkownik.Id,
                Imie = uzytkownik.Imie,
                Nazwisko = uzytkownik.Nazwisko,
                Email = uzytkownik.Email,
                Role = uzytkownik.Role,
                IdPrzelozonego = uzytkownik.IdPrzelozonego
            };
        }

        public UzytkownikDTO Create(UzytkownikDTO obj)
        {

            if (obj.Role < 1 || obj.Role > 2 || obj.Role == null)
            {
                throw new System.ArgumentException("Prawidlowa wartość roli: 1 lub 2");
            }
            if (obj.IdPrzelozonego < 1 || obj.IdPrzelozonego == null)
            {
                throw new System.ArgumentException("IdPrzelozonego nie moze byc zerem lub NULL");
            }




            try
            {
                _context.Uzytkownik.Add(ConvertToModel(obj));
                _context.SaveChanges();
            }
            catch (DbUpdateException e)
            {
                throw new DbUpdateException(e.Message);
            }




            return obj;
        }

        public UzytkownikDTO Delete(int id)
        {
            try
            {
                var user = _context.Uzytkownik.Find(id);
                if (user == null)
                {
                    throw new NullReferenceException();
                }
                _context.Uzytkownik.Remove(user);
                return ConvertToDTO(user);
            }
            catch (NullReferenceException)
            {
                throw new NullReferenceException("User not found");
            }
        }
        public List<UzytkownikDTO> Read()
        {
            //return all users converted to DTO
            var user = _context.Uzytkownik
               .Include(x => x.IdPrzelozonegoNavigation)
               .Include(x => x.RoleNavigation)
               .Select(x => ConvertToDTO(x));
            return user
               .ToList();

        }
        public UzytkownikDTO Read(int id)
        {
            var user = _context.Uzytkownik
                .Include(x => x.IdPrzelozonegoNavigation)
                .Include(x => x.RoleNavigation)
                .FirstOrDefault(x => x.Id == id);
            if (user == null)
            {
                throw new NullReferenceException();
            }
            return ConvertToDTO(user);
        }



        public UzytkownikDTO Update(int id, UzytkownikDTO obj)
        {

            var user = _context.Uzytkownik.Find(id);
            if (user == null)
            {
                throw new NullReferenceException();
            }
            try
            {
                foreach (var propetryEntry in _context.Entry(user).Properties)
                {
                    if (propetryEntry.Metadata.Name != "Id")
                    {
                        propetryEntry.CurrentValue = obj.GetType().GetProperty(propetryEntry.Metadata.Name).GetValue(obj);
                    }
                }
                _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                throw new DbUpdateException("Updating user failed");
            }

            return obj;

        }


    }
}
