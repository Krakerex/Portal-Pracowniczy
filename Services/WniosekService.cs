using krzysztofb.Converters;
using krzysztofb.CustomExceptions;
using krzysztofb.Email;
using krzysztofb.Models;
using krzysztofb.Models.DTO;
using krzysztofb.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace krzysztofb.Services
{
    /// <summary>
    /// Service służący do obsługi wniosków
    /// </summary>
    public class WniosekService :
        IDatabaseDelete<WniosekDTO>,
        IDatabaseFileSave<WniosekDTO>,
        IPdfToDatabaseCreate<Wniosek>,
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
        /// <summary>
        /// Metoda służąca do walidacji i dodania pliku jak pole obiektu WniosekDTO
        /// </summary>
        /// <param name="wniosek">Wcześniej przygotowany obiekt WniosekDTO, do którego chcemy dodać plik</param>
        /// <param name="file">Plik, który chcemy dodać</param>
        /// <returns>WniosekDTO z dodanym plikiem</returns>
        /// <exception cref="BadHttpRequestException"></exception>
        public WniosekDTO AddFile(WniosekDTO wniosek, IFormFile file)
        {
            if (file == null)
            {
                throw new UploadException("Brak pliku");
            }
            if (_context.Uzytkownik.Find(wniosek.IdOsobyZglaszajacej) == null)
            {
                throw new DatabaseValidationException("Użytkownik wysyłający wniosek nie istnieje");
            }
            file.CopyToAsync(_memoryStream);
            wniosek.Nazwa = file.FileName;
            wniosek.Plik = _memoryStream.ToArray();
            return wniosek;
        }
        /// <summary>
        /// Metoda zapisująca obiekt wniosekDTO do bazy danych
        /// </summary>
        /// <param name="obj">Obiekt wniosekDTO do zapisania</param>
        /// <returns>Zapisany wniosekDTO</returns>
        public Wniosek Create(IFormFile file)
        {
            WniosekDTO wniosek;
            string[] imieINazwisko;

            using (StringReader sr = IFormFileToStringReader.LoadPdf(file))
            {
                wniosek = IFormFileToStringReader.GetPdfData(sr);
            }
            using (StringReader sr = IFormFileToStringReader.LoadPdf(file))
            {
                imieINazwisko = IFormFileToStringReader.GetWniosekSender(sr);
                int id_osoby_zglaszajacej = _context.Uzytkownik.Where(x => x.Imie == imieINazwisko[0] && x.Nazwisko == imieINazwisko[1]).FirstOrDefault().Id;
                if (id_osoby_zglaszajacej == null)
                {
                    throw new DatabaseValidationException("Użytkownik o podanym imieniu i nazwisku nie istnieje");
                }
                else
                {
                    wniosek.IdOsobyZglaszajacej = id_osoby_zglaszajacej;
                }
            }
            wniosek = AddFile(wniosek, file);
            var entry = _context.Wniosek.Add(ModelConverter.ConvertToModel(wniosek));
            _context.SaveChanges();
            _context.Entry(entry.Entity).GetDatabaseValues();
            return entry.Entity;
        }
        /// <summary>
        /// Metoda walidująca i usuwająca wniosek o podanym id
        /// </summary>
        /// <param name="id">Id wniosku do usunięcia</param>
        /// <returns>Usunięty wniosek</returns>
        /// <exception cref="BadHttpRequestException"></exception>
        public WniosekDTO Delete(int id)
        {
            var wniosek = _context.Wniosek.Find(id);
            if (wniosek == null)
            {
                throw new DatabaseValidationException("Wniosek do usunięcia nie znaleziony");
            }

            _context.Wniosek.Remove(wniosek);
            _context.SaveChanges();
            return ModelConverter.ConvertToDTO(wniosek);
        }
        /// <summary>
        /// Metoda wczytująca wszystkie wnioski z bazy danych do listy
        /// </summary>
        /// <returns>Lista obiektów WniosekDTO</returns>
        public List<Wniosek> Read()
        {
            //read all from model Wniosek
            var wnioski = _context.Wniosek
               .Include(x => x.IdOsobyAkceptujacejNavigation)
               .Include(x => x.IdOsobyZglaszajacejNavigation)
               .Select(x => x);
            return wnioski
               .ToList();
        }
        /// <summary>
        /// Metoda walidująca i zmieniająca status wniosku na zaakceptowany
        /// </summary>
        /// <param name="idWniosek">Id wniosku do zaakceptowania</param>
        /// <param name="idKierownik">Id kierownika akceptującego wniosek</param>
        /// <returns>Zaakceptowany WniosekDTO</returns>
        /// <exception cref="BadHttpRequestException"></exception>
        public WniosekDTO Accept(int idWniosek, int idKierownik)
        {
            //check if user with idKierownik has role Kierownik
            var wniosek = _context.Wniosek.Find(idWniosek);
            var kierownik = _context.Uzytkownik.Find(idKierownik);
            switch (kierownik)
            {
                case null:
                    throw new DatabaseValidationException("Nie podano id kierownika");
                default:
                    if (kierownik.Role != 2)
                    {
                        throw new DatabaseValidationException("Użytkownik o id: " + idKierownik + " nie jest kierownikiem");
                    }
                    else if (wniosek == null)
                    {
                        throw new DatabaseValidationException("Wniosek o id: " + idWniosek + " nie istnieje");
                    }

                    break;
            }
            wniosek.IdOsobyAkceptujacej = idKierownik;
            UzytkownikDTO osobaZglaszajaca = _uzytkownikService.Read(wniosek.IdOsobyZglaszajacej.Value);
            UzytkownikDTO osobaAkceptujaca = _uzytkownikService.Read(idKierownik);
            UzytkownikDTO przelozony = _uzytkownikService.Read(osobaZglaszajaca.IdPrzelozonego.Value);
            var file = ReadFile(idWniosek);
            IFormFileCollection attachments = new FormFileCollection() { file };
            EmailDataWniosekUrlop email = EmailDataWniosekUrlop.BuildMail(wniosek, osobaZglaszajaca, osobaAkceptujaca, przelozony, attachments);
            _emailService.SendEmailWithAttachment(email);
            _context.SaveChanges();

            return ModelConverter.ConvertToDTO(wniosek);
        }
        /// <summary>
        /// Metoda walidująca i wczytująca plik z wniosku o podanym id
        /// </summary>
        /// <param name="id">Id wniosku z którego chcemy wczytać plik</param>
        /// <returns>IFormFIle pliku wczytanego z bazy</returns>
        /// <exception cref="BadHttpRequestException"></exception>
        public IFormFile ReadFile(int id)
        {
            var wniosek = _context.Wniosek.Find(id);
            if (wniosek == null)
            {
                throw new DatabaseValidationException("Wniosek o id: " + id + " nie istnieje");
            }
            var stream = new MemoryStream(wniosek.Plik);
            IFormFile file = new FormFile(stream, 0, wniosek.Plik.Length, wniosek.Nazwa, wniosek.Nazwa)
            {
                Headers = new HeaderDictionary(),
                ContentType = "application/pdf",
                ContentDisposition = "attachment"
            };
            return file;
        }
        /// <summary>
        /// Metoda pobierająca plik z wnioskiem na podstawie id
        /// </summary>
        /// <param name="id">Id wniosku z plikiem do pobrania</param>
        /// <returns>FileResult pliku do pobrania</returns>
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
