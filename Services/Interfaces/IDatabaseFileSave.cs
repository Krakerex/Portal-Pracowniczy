namespace krzysztofb.Services.Interfaces
{
    /// <summary>
    /// Interfejs zawierający metodę zapisującą plik do obiektu
    /// </summary>
    /// <typeparam name="T">Typ obiektu w którym zapisujemy plik</typeparam>
    public interface IDatabaseFileSave<T>
    {
        /// <summary>
        /// Metoda zapisująca plik w obiekcie
        /// </summary>
        /// <param name="wniosek">Obiekt do którego zapisujemy plik</param>
        /// <param name="file">IFormFile pliku do zapisu</param>
        /// <returns>Obiekt z zapisanym plikiem</returns>
        public T AddFile(T wniosek, IFormFile file);
    }
}
