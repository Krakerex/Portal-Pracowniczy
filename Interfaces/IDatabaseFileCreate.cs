namespace krzysztofb.Interfaces
{
    public interface IDatabaseFileCreate<T>
    {
        public T Create(IFormFile file);
    }
}
