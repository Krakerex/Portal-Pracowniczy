namespace krzysztofb.Interfaces
{
    /// <summary>
    /// Interfejs zawierający metodę usuwającą obiekt z bazy danych
    /// </summary>
    /// <typeparam name="T">Typ usuwanego obiektu</typeparam>
    public interface IDatabaseDelete<T>
    {
        /// <summary>
        /// Metoda usuwająca obiekt z bazy danych
        /// </summary>
        /// <param name="id">id usuwanego obiektu</param>
        /// <returns>Usunięty obiekt</returns>
        public T Delete(int id);
    }
}
