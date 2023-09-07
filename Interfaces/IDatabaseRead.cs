namespace krzysztofb.Interfaces
{
    public interface IDatabaseRead<T>
    {
        public List<T> Read();
        public T Read(int id);

    }
}
