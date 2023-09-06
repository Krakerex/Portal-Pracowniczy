namespace krzysztofb.Models.DTO
{
    public interface IModelConverter<T2, T>
    {
        static abstract T ConvertToDTO(T2 uzytkownik);
        static abstract T2 ConvertToModel(T uzytkownik);
    }
}
