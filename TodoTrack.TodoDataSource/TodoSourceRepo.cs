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
            return new Project { Id= Guid.NewGuid().ToString(), Name = value };
        }

        public async Task<IList<TodoItem>> GetTodayTodoItemsAsync()
        {
            return _dbContext.TodoItems.ToList();
        }

        public async Task<bool> PostNewEntriesAsync(IEnumerable<ProcessPeriod> workFromProcesses)
        {
            _dbContext.WorkFromProcesses.AddRange(workFromProcesses);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public Task<bool> UpdateTodoItemAsync(string id, TodoItem item)
        {
            throw new NotImplementedException();
        }
    }
}