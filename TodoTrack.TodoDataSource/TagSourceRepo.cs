using TodoTrack.Contracts;

namespace TodoTrack.TodoDataSource
{
    /// <summary>
    /// this is temporary use case.
    /// </summary>
    public class TagSourceRepo : IRepo<Tag>
    {
        private readonly TodoDbContext _dbContext;
        public TagSourceRepo()
        {
            _dbContext = new TodoDbContext();
            //_dbContext.Database.EnsureDeleted();
            _dbContext.Database.EnsureCreated();
        }
        //TODO: USE DTO TO MAKE ID IMUTTABLE
        public async Task<Tag> CreateAsync(Tag item)
        {
            item.Id = Guid.NewGuid().ToString();
            _dbContext.Tags.Add(item);
            await _dbContext.SaveChangesAsync();
            return item;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var item = _dbContext.Tags.Find(id);
            if (item == null) return false;
            _dbContext.Tags.Remove(item);
            await _dbContext.SaveChangesAsync(true);
            return true;
        }

        public async Task<IQueryable<Tag>> GetAsync()
        {
            return await Task.FromResult(_dbContext.Tags);
        }

        public async Task<Tag?> UpdateAsync(string id, Tag item)
        {
            var target = _dbContext.Tags.Find(id);
            if (target == null) return null;
            _dbContext.Entry(target).CurrentValues.SetValues(item);
            await _dbContext.SaveChangesAsync();
            return target;
        }
    }
}