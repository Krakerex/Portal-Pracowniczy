namespace krzysztofb.Services.Interfaces
{
    /// <summary>
    /// Interfejs zawierający metodę tworzącą obiekt w bazie danych
    /// </summary>
    /// <typeparam name="T">Typ tworzonego obiektu</typeparam>
    public interface IDatabaseCreate<T, T2>
    {
        /// <summary>
        /// Metoda tworząca obiekt w bazie danych
        /// </summary>
        /// <param name="obj">Obiekt do dodania</param>
        /// <returns>Dodany obiekt</returns>
        public T2 Create(T obj);
    }
}