namespace krzysztofb.Services.Interfaces
{
    public interface IPdfToDatabaseCreate<T>
    {
        public T Create(IFormFile file);

    }
}
