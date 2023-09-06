namespace krzysztofb.Interfaces
    {
    public interface IDatabaseUpdate<T>
        {
        public T Update(int id,T obj);
        }
    }
