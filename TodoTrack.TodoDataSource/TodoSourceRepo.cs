using ForegroundTimeTracker.Models;
using TodoTrack.Contracts;

namespace TodoTrack.TodoDataSource
{
    /// <summary>
    /// this is temporary use case.
    /// </summary>
    public class TodoSourceRepo : ITodoRepo, IWorkFromProcessRepo
    {
        private readonly SQLiteDbContext _dbContext;
        public TodoSourceRepo()
        {
            _dbContext = new SQLiteDbContext();
            //_dbContext.Database.EnsureDeleted();
            _dbContext.Database.EnsureCreated();
        }
        //TODO: USE DTO TO MAKE ID IMUTTABLE
        public async Task<TodoItem> CreateTodoItemAsync(TodoItem item)
        {
            item.Id = Guid.NewGuid().ToString();
            _dbContext.TodoItems.Add(item);
            await _dbContext.SaveChangesAsync();
            return item;
        }

        public async Task<bool> DeleteTodoItemAsync(string id)
        {
            var item = _dbContext.TodoItems.Find(id);
            if (item == null) return false;
            _dbContext.TodoItems.Remove(item);
            await _dbContext.SaveChangesAsync(true);
            return true;
        }

        public async Task<Project?> GetProjectFromNameAsync(string value)
        {
            return await Task.FromResult(new Project { Id= Guid.NewGuid().ToString(), Name = value });
        }

        public async Task<IList<TodoItem>> GetTodayTodoItemsAsync()
        {
            return await Task.FromResult(_dbContext.TodoItems.ToList());
        }

        public async Task<bool> PostNewEntriesAsync(IEnumerable<ProcessPeriod> workFromProcesses)
        {
            _dbContext.WorkFromProcesses.AddRange(workFromProcesses);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateTodoItemAsync(string id, TodoItem item)
        {
            var target = _dbContext.TodoItems.Find(id);
            if (target == null) return false;
            _dbContext.Entry(target).CurrentValues.SetValues(item);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}