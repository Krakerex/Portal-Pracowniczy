using krzysztofb.Models;
using krzysztofb.Models.DTO;

namespace krzysztofb.Email
{
    public class EmailDataWniosek : EmailDataWithAttachment
    {
        public EmailDataWniosek BuildMail(Wniosek wniosek, UzytkownikDTO osobaZglaszajaca, UzytkownikDTO osobaAkceptujaca, UzytkownikDTO przelozony, IFormFileCollection attachments)
        {
            return new EmailDataWniosek
            {
                EmailToId = przelozony.Email,
                EmailToName = przelozony.Imie + " " + przelozony.Nazwisko,
                EmailBody = "Wniosek o nazwie: " + wniosek.Nazwa +
           "\n Użytkownika: " + osobaZglaszajaca.Imie + " " + osobaZglaszajaca.Nazwisko +
           "\n Został zaakceptowany przez: " + osobaAkceptujaca.Imie + " " + osobaAkceptujaca.Nazwisko,
                EmailSubject = "Wniosek " + osobaZglaszajaca.Imie + " " + osobaZglaszajaca.Nazwisko + " został zaakceptowany",
                EmailAttachments = attachments
            };
        }
    }
}
