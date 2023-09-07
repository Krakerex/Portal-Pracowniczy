namespace krzysztofb.Interfaces
{
    public interface IDatabaseFileSave<T>
    {
        public T AddFile(T wniosek, IFormFile file);
    }
}
