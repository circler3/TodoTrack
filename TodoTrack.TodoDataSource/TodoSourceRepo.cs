using ForegroundTimeTracker.Models;
using TodoTrack.Contracts;

namespace TodoTrack.TodoDataSource
{
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
            return true;
        }

        public bool UpdateAsync(TodoItem item)
        {
            return true;
        }
    }
}