namespace krzysztofb.Services.Interfaces
{
    /// <summary>
    /// Interfejs zawierający metodę odczytującą obiekt z bazy danych
    /// </summary>
    /// <typeparam name="T">Typ obiektu do odczytania</typeparam>
    public interface IDatabaseRead<T>
    {
        /// <summary>
        /// Metoda odczytująca wszystkie obiekty z bazy danych
        /// </summary>
        /// <returns>Lista obiektów odczytanych</returns>
        public List<T> Read();
        /// <summary>
        /// Metoda odczytująca obiekt z bazy danych
        /// </summary>
        /// <param name="id">Id obiektu do odczytania</param>
        /// <returns>Odczytany obiekt</returns>
        public T Read(int id);

    }
}
