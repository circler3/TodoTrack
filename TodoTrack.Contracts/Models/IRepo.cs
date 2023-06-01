namespace TodoTrack.Contracts
{
    public interface IRepo<T> : IRepo
        where T : class, IEntity
    {
        Task<IQueryable<T>> GetAsync();
        Task<bool> DeleteAsync(string id);
        Task<T> CreateAsync(T item);
        Task<bool> UpdateAsync(string id, T item);
    }

    public interface IRepo
    {
        
    }
}