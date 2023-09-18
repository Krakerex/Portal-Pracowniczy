using Microsoft.AspNetCore.Mvc;

namespace krzysztofb.Services.Interfaces
{
    /// <summary>
    /// Interfejs zawierający metody do odczytu i pobierania plików z bazy danych
    /// </summary>
    public interface IDatabaseFileRead
    {
        /// <summary>
        /// Metoda odczytująca plik z bazy danych
        /// </summary>
        /// <param name="id">Id wniosku z którego odczytujemy plik</param>
        /// <returns>IFormFile odczytanego pliku</returns>
        public IFormFile ReadFile(int id);
        /// <summary>
        /// Metoda pobierająca plik z bazy danych
        /// </summary>
        /// <param name="id">Id wniosku z którego pobieramy plik</param>
        /// <returns>FileResult pobranego pliku</returns>
        public FileResult DownloadFile(int id);
    }
}
