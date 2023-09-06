using krzysztofb.Interfaces;
using krzysztofb.Models;
using krzysztofb.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace krzysztofb
{
    public class WniosekTable :

        IDatabaseRead<WniosekDTO, WniosekDTO>,
        IDatabaseUpdate<WniosekDTO>,
        IDatabaseDelete<WniosekDTO>,
        IDatabaseFileCreate<WniosekDTO>,
        IModelConverter<Wniosek, WniosekDTO>
    {
        private readonly WnioskiContext _context;
        private readonly MemoryStream _memoryStream;
        public WniosekTable(WnioskiContext context, MemoryStream memoryStream)
        {
            _context = context;
            _memoryStream = memoryStream;
        }
        public static WniosekDTO ConvertToDTO(Wniosek wniosek)
        {
            return new WniosekDTO
            {
                Id = wniosek.Id,
                Plik = wniosek.Plik,
                Nazwa = wniosek.Nazwa,
                IdOsobyZglaszajacej = wniosek.IdOsobyZglaszajacej,
                IdOsobyAkceptujacej = wniosek.IdOsobyAkceptujacej
            };
        }

        public static Wniosek ConvertToModel(WniosekDTO wniosek)
        {
            return new Wniosek
            {
                Id = wniosek.Id,
                Plik = wniosek.Plik,
                Nazwa = wniosek.Nazwa,
                IdOsobyZglaszajacej = wniosek.IdOsobyZglaszajacej,
                IdOsobyAkceptujacej = wniosek.IdOsobyAkceptujacej
            };
        }


        public WniosekDTO Create(IFormFile file)
        {
            file.CopyToAsync(_memoryStream);
            var wniosek = new WniosekDTO
            {
                Nazwa = file.FileName,
                Plik = _memoryStream.ToArray(),
                IdOsobyZglaszajacej = 1,
                IdOsobyAkceptujacej = 1
            };
            _context.Wniosek.Add(ConvertToModel(wniosek));
            _context.SaveChanges();
            return wniosek;
        }

        public WniosekDTO Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<WniosekDTO> Read()
        {
            //read all from model Wniosek
            var wnioski = _context.Wniosek
               .Include(x => x.IdOsobyAkceptujacejNavigation)
               .Include(x => x.IdOsobyZglaszajacejNavigation)
               .Select(x => ConvertToDTO(x));
            return wnioski
               .ToList();
        }

        public WniosekDTO Read(int id)
        {
            //read field plik from model Wniosek and convert it from byte[] to IFormFile
            var wniosek = _context.Wniosek.Find(id);
            if (wniosek == null)
            {
                throw new NullReferenceException();
            }
            return ConvertToDTO(wniosek);
        }

        public WniosekDTO Update(int id, WniosekDTO obj)
        {
            throw new NotImplementedException();
        }
    }
}
