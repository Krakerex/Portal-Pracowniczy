using krzysztofb.Email;

namespace krzysztofb.Interfaces
{
    /// <summary>
    /// Interfejs zawierający metody do wysyłania wiadomości email
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Metoda wysyłająca wiadomość email
        /// </summary>
        /// <param name="emailData">EmailData zawierający wiadomość</param>
        /// <returns>Bool zależny od sukcesu wysłania</returns>
        bool SendEmail(EmailData emailData);
        /// <summary>
        /// Metoda wysyłająca wiadomość email z załącznikiem
        /// </summary>
        /// <param name="emailData">EmailData zawierający wiadomość z załącznikami</param>
        /// <returns>Bool zależny od sukcesu wysłania</returns>
        bool SendEmailWithAttachment(EmailDataWithAttachment emailData);
    }
}

