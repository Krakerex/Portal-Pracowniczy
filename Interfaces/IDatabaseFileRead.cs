namespace krzysztofb.Interfaces
{
    public interface IDatabaseFileRead<T>
    {
        public IFormFile Read(T file);
    }
}
