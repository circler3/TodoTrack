namespace TodoTrack.Contracts
{
    public interface IRepo<T>
        where T : class, IEntity
    {
        Task<IQueryable<T>> GetAsync();
        Task<bool> DeleteAsync(string id);
        Task<T> CreateAsync(T item);
        Task<bool> UpdateAsync(string id, T item);
    }
}