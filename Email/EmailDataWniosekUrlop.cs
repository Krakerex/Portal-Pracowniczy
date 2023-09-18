using krzysztofb.Models;
using krzysztofb.Models.DTO;

namespace krzysztofb.Email
{
    /// <summary>
    /// Model zawierający dane do wysłania wiadomości email rozszerzony o możliwość dodania załącznika przystosowany do wysyłania wiadomości o zaakceptowaniu wniosku
    /// </summary>
    public class EmailDataWniosekUrlop : EmailDataWithAttachment
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
        public static EmailDataWniosekUrlop BuildMail(Wniosek wniosek, UzytkownikDTO osobaZglaszajaca, UzytkownikDTO osobaAkceptujaca, UzytkownikDTO przelozony, IFormFileCollection attachments)
        {

            return new EmailDataWniosekUrlop
            {
                EmailToId = przelozony.Email,
                EmailToName = przelozony.Imie + " " + przelozony.Nazwisko,
                EmailBody = "Wniosek o urlop: " + wniosek.Nazwa + "\no numerze ewidencyjnym: " + wniosek.NumerEwidencyjny +
                "\nUżytkownika: " + osobaZglaszajaca.Imie + " " + osobaZglaszajaca.Nazwisko +
                " \nwypełniony dnia: " + wniosek.DataWypelnienia.Value.ToShortDateString() + " \nw liczbie dni: " + wniosek.IloscDni +
                "\nod: " + wniosek.DataRozpoczecia.Value.ToShortDateString() + " do: " + wniosek.DataZakonczenia.Value.ToShortDateString() +
           "\nZostał zaakceptowany przez: " + osobaAkceptujaca.Imie + " " + osobaAkceptujaca.Nazwisko,
                EmailSubject = "Wniosek " + osobaZglaszajaca.Imie + " " + osobaZglaszajaca.Nazwisko + " został zaakceptowany",
                EmailAttachments = attachments
            };
        }
    }
}
