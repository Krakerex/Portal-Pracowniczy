using krzysztofb.Models;
using krzysztofb.Models.DTO;

namespace krzysztofb.Email
{
    /// <summary>
    /// Model zawierający dane do wysłania wiadomości email rozszerzony o możliwość dodania załącznika przystosowany do wysyłania wiadomości o zaakceptowaniu wniosku
    /// </summary>
    public class EmailDataWniosek : EmailDataWithAttachment
    {
        /// <summary>
        /// Metoda budująca wiadomość email o zaakceptowaniu wniosku
        /// </summary>
        /// <param name="wniosek">Objekt WniosekDTO zawierający dane wniosku</param>
        /// <param name="osobaZglaszajaca">Obiekt UżytkownikDTO zawierający dane osoby zgłaszającej wniosek</param>
        /// <param name="osobaAkceptujaca">Obiekt UżytkownikDTO zawierający dane osoby akceptującej wniosek</param>
        /// <param name="przelozony">Obiekt UżytkownikDTo zawierający dane przełożonego osoby zgłaszającej</param>
        /// <param name="attachments">IformFileCollection zawierający załączniki do maila</param>
        /// <returns></returns>
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
