using TodoTrack.Contracts;

namespace TodoTrack.TodoDataSource
{
    /// <summary>
    /// this is temporary use case.
    /// </summary>
    public class ProcessPeriodSourceRepo : IRepo<ProcessPeriod>
    {
        private readonly TodoDbContext _dbContext;

        public ProcessPeriodSourceRepo()
        {
            _dbContext = new TodoDbContext();
            //_dbContext.Database.EnsureDeleted();
            _dbContext.Database.EnsureCreated();
        }
        //TODO: USE DTO TO MAKE ID IMUTTABLE
        public async Task<ProcessPeriod> CreateAsync(ProcessPeriod item)
        {
            item.Id = Guid.NewGuid().ToString();
            _dbContext.ProcessPeriods.Add(item);
            await _dbContext.SaveChangesAsync();
            return item;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var item = _dbContext.ProcessPeriods.Find(id);
            if (item == null) return false;
            _dbContext.ProcessPeriods.Remove(item);
            await _dbContext.SaveChangesAsync(true);
            return true;
        }

        public async Task<IQueryable<ProcessPeriod>> GetAsync()
        {
            return await Task.FromResult(_dbContext.ProcessPeriods);
        }

        public async Task<ProcessPeriod?> UpdateAsync(string id, ProcessPeriod item)
        {
            var target = _dbContext.ProcessPeriods.Find(id);
            if (target == null) return null;
            _dbContext.Entry(target).CurrentValues.SetValues(item);
            await _dbContext.SaveChangesAsync();
            return target;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_dbContext != null)
                {
                    _dbContext.Dispose();
                    //_dbContext = null;
                }
            }
        }
    }
}