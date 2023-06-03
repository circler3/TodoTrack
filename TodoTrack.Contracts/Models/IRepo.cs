namespace TodoTrack.Contracts
{
    public interface IRepo<T>
        where T : class, IEntity
    {
        Task<IQueryable<T>> GetAsync();
        Task<bool> DeleteAsync(string id);
        Task<T> CreateAsync(T item);
        Task<T?> UpdateAsync(string id, T item);
    }

    public interface IRepo
    {
        Task<IQueryable> GetAsync();
        //Task DeleteAsync(string id);
        // Task CreateAsync(IEntity item);
        // Task UpdateAsync(string id, IEntity item);
    }
}