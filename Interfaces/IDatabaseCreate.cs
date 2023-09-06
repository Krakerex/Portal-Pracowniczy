namespace krzysztofb.Interfaces
{
    public interface IDatabaseCreate<T>
    {
        public T Create(T obj);
    }
}
