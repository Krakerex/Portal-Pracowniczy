namespace krzysztofb.Interfaces
{
    public interface IDatabaseRead<T2, T>
    {
        public List<T> Read();
        public T2 Read(int id);

    }
}
