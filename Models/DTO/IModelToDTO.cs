namespace krzysztofb.Models.DTO
{
    public interface IModelConverter<T2, T>
    {
        static abstract T ConvertToDTO(T2 obj);
        static abstract T2 ConvertToModel(T obj);
    }
}
