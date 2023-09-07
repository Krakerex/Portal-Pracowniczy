using krzysztofb.Email;
using krzysztofb.Interfaces;
using krzysztofb.Models;
using krzysztofb.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace krzysztofb.Services
{
    public class WniosekService :
        IDatabaseDelete<WniosekDTO>,
        IDatabaseCreate<WniosekDTO>,
        IDatabaseFileSave<WniosekDTO>,
        IDatabaseFileRead
    {
        private readonly WnioskiContext _context;
        private readonly MemoryStream _memoryStream;
        private readonly UzytkownikService _uzytkownikService;
        IEmailService _emailService = null;
        public WniosekService(WnioskiContext context, MemoryStream memoryStream, UzytkownikService userTable, IEmailService emailService)
        {
            _context = context;
            _memoryStream = memoryStream;
            _uzytkownikService = userTable;
            _emailService = emailService;

        }

        public WniosekDTO AddFile(WniosekDTO wniosek, IFormFile file)
        {
            file.CopyToAsync(_memoryStream);
            wniosek.Nazwa = file.FileName;
            wniosek.Plik = _memoryStream.ToArray();
            return wniosek;
        }

        public WniosekDTO Create(WniosekDTO obj)
        {
            var entry = _context.Wniosek.Add(ModelConverter.ConvertToModel(obj));
            _context.SaveChanges();
            _context.Entry(entry.Entity).GetDatabaseValues();
            return ModelConverter.ConvertToDTO(entry.Entity);
        }

        public WniosekDTO Delete(int id)
        {
            var wniosek = _context.Wniosek.Find(id);
            _context.Wniosek.Remove(wniosek);
            _context.SaveChanges();
            return ModelConverter.ConvertToDTO(wniosek);
        }

        public List<WniosekDTO> Read()
        {
            //read all from model Wniosek
            var wnioski = _context.Wniosek
               .Include(x => x.IdOsobyAkceptujacejNavigation)
               .Include(x => x.IdOsobyZglaszajacejNavigation)
               .Select(x => ModelConverter.ConvertToDTO(x));
            return wnioski
               .ToList();
        }
        public WniosekDTO Accept(int idWniosek, int idKierownik)
        {
            var wniosek = _context.Wniosek.Find(idWniosek);
            UzytkownikDTO osobaZglaszajaca = _uzytkownikService.Read(wniosek.IdOsobyZglaszajacej.Value);
            UzytkownikDTO osobaAkceptujaca = _uzytkownikService.Read(wniosek.IdOsobyAkceptujacej.Value);
            UzytkownikDTO przelozony = _uzytkownikService.Read(osobaZglaszajaca.IdPrzelozonego.Value);
            var file = ReadFile(idWniosek);
            IFormFileCollection attachments = new FormFileCollection() { file };
            EmailDataWniosek email = new EmailDataWniosek().BuildMail(wniosek, osobaZglaszajaca, osobaAkceptujaca, przelozony, attachments);
            _emailService.SendEmailWithAttachment(email);
            _context.SaveChanges();

            return ModelConverter.ConvertToDTO(wniosek);
        }
        public IFormFile ReadFile(int id)
        {
            var wniosek = _context.Wniosek.Find(id);
            var stream = new MemoryStream(wniosek.Plik);
            IFormFile file = new FormFile(stream, 0, wniosek.Plik.Length, wniosek.Nazwa, wniosek.Nazwa)
            {
                Headers = new HeaderDictionary(),
                ContentType = "application/pdf",
                ContentDisposition = "attachment"
            };
            return file;
        }
        public FileResult DownloadFile(int id)
        {
            byte[] file;
            var wniosek = ReadFile(id);
            using (MemoryStream memoryStream = new MemoryStream())
            {
                wniosek.CopyTo(memoryStream);
                file = memoryStream.ToArray();
            }
            FileResult fileResult = new FileContentResult(file, "application/pdf");
            fileResult.FileDownloadName = wniosek.FileName;
            return fileResult;
        }
    }
}
