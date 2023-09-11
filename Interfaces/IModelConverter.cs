namespace krzysztofb.Interfaces
{
    /// <summary>
    /// Interfejs konwertera modelu na DTO i odwrotnie
    /// </summary>
    /// <typeparam name="T2">Typ obiektu</typeparam>
    /// <typeparam name="T">Typ obiektu DTO</typeparam>
    public interface IModelConverter<T2, T>
    {
        /// <summary>
        /// Metoda konwertująca obiekt na DTO
        /// </summary>
        /// <param name="obj">Obiekt do przekonwertowania</param>
        /// <returns>Przekonwertowany Obiekt DTO</returns>
        static abstract T ConvertToDTO(T2 obj);
        /// <summary>
        /// Metoda konwertująca obiekt DTO na obiekt
        /// </summary>
        /// <param name="obj">Obiekt DTO do przekonwertowania</param>
        /// <returns>Obiekt</returns>
        static abstract T2 ConvertToModel(T obj);
    }
}
