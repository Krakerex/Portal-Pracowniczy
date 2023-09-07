using krzysztofb.Email;
using krzysztofb.Interfaces;
using krzysztofb.Models;
using krzysztofb.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace krzysztofb
{
    public class WniosekTable :

        IDatabaseRead<WniosekDTO>,
        IDatabaseUpdate<WniosekDTO>,
        IDatabaseDelete<WniosekDTO>,
        IModelConverter<Wniosek, WniosekDTO>,
        IDatabaseCreate<WniosekDTO>
    {
        private readonly WnioskiContext _context;
        private readonly MemoryStream _memoryStream;
        private readonly UserTable _userTable;
        IEmailService _emailService = null;
        public WniosekTable(WnioskiContext context, MemoryStream memoryStream, UserTable userTable, IEmailService emailService)
        {
            _context = context;
            _memoryStream = memoryStream;
            _userTable = userTable;
            _emailService = emailService;

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


        public WniosekDTO SaveFile(WniosekDTO wniosek, IFormFile file)
        {
            file.CopyToAsync(_memoryStream);
            wniosek.Nazwa = file.FileName;
            wniosek.Plik = _memoryStream.ToArray();
            _context.Wniosek.Add(ConvertToModel(wniosek));
            _context.SaveChanges();
            return wniosek;
        }

        public WniosekDTO Create(WniosekDTO obj)
        {
            _context.Wniosek.Add(ConvertToModel(obj));
            _context.SaveChanges();
            return obj;
        }

        public WniosekDTO Delete(int id)
        {
            try
            {
                var wniosek = _context.Wniosek.Find(id);
                if (wniosek == null)
                {
                    throw new NullReferenceException();
                }
                _context.Wniosek.Remove(wniosek);
                _context.SaveChanges();
                return ConvertToDTO(wniosek);
            }
            catch (NullReferenceException)
            {
                throw new NullReferenceException("Wniosek not found");
            }
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
        public WniosekDTO Accept(int idWniosek, int idKierownik)
        {
            var wniosek = _context.Wniosek.Find(idWniosek);
            if (wniosek == null)
            {
                throw new NullReferenceException();
            }
            wniosek.IdOsobyAkceptujacej = idKierownik;
            UzytkownikDTO osobaZglaszajaca = _userTable.Read((int)wniosek.IdOsobyZglaszajacej);
            UzytkownikDTO osobaAkceptujaca = _userTable.Read((int)wniosek.IdOsobyAkceptujacej);
            UzytkownikDTO przelozony = _userTable.Read((int)osobaZglaszajaca.IdPrzelozonego);
            var stream = new MemoryStream(wniosek.Plik);
            IFormFile file = new FormFile(stream, 0, wniosek.Plik.Length, wniosek.Nazwa, wniosek.Nazwa)
            {
                Headers = new HeaderDictionary(),
                ContentType = "application/pdf",
                ContentDisposition = "attachment"
            };


            IFormFileCollection attachments = new FormFileCollection() { file };
            EmailDataWithAttachment email = new EmailDataWithAttachment
            {
                EmailToId = osobaZglaszajaca.Email,
                EmailToName = osobaZglaszajaca.Imie + " " + osobaZglaszajaca.Nazwisko,
                EmailBody = "Wniosek o nazwie: " + wniosek.Nazwa +
           "\n Użytkownika: " + osobaZglaszajaca.Imie + " " + osobaZglaszajaca.Nazwisko +
           "\n Został zaakceptowany przez: " + osobaAkceptujaca.Imie + " " + osobaAkceptujaca.Nazwisko,
                EmailSubject = "Wniosek " + osobaZglaszajaca.Imie + " " + osobaZglaszajaca.Nazwisko + " został zaakceptowany",
                EmailAttachments = attachments

            };
            _emailService.SendEmailWithAttachment(email);
            _context.SaveChanges();

            return ConvertToDTO(wniosek);
        }
    }
}
