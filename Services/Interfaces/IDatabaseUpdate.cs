namespace krzysztofb.Services.Interfaces
{
    /// <summary>
    /// Interfejs zawierający metodę aktualizującą obiekt w bazie danych
    /// </summary>
    /// <typeparam name="T">Typ obiektu do zaaktualizowania</typeparam>
    public interface IDatabaseUpdate<T>
    {
        /// <summary>
        /// Metoda aktualizująca obiekt w bazie danych
        /// </summary>
        /// <param name="id">Id obiektu do zaaktualizowania</param>
        /// <param name="obj">Obiekt z nowymi danymi</param>
        /// <returns>Zaaktualizowany obiekt</returns>
        public T Update(int id, T obj);
    }
}
