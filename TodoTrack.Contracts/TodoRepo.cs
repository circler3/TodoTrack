using TodoTrack.Contracts;

namespace TodoTrack.Contracts
{
    public class TodoRepo : ITodoRepo
    {
        public async ValueTask<TodoItem> GetCurrentTodoItemAsync()
        {
            return await ValueTask.FromResult(new TodoItem { Name = "Test" });
        }

        public async ValueTask<IList<TodoItem>> GetTodayTodoItemsAsync()
        {
            return await ValueTask.FromResult( new[] { new TodoItem { Name = "Test1" },
                new TodoItem { Name = "Test2" },
                 new TodoItem { Name = "Test3" },
                  new TodoItem { Name = "Test4" },
                   new TodoItem { Name = "Test5" },
                   new TodoItem { Name = "Test6" }
            });
        }

        public bool UpdateAsync(TodoItem item)
        {
            return true;
        }

        Task<TodoItem> ITodoRepo.CreateTodoItemAsync(TodoItem item)
        {
            throw new NotImplementedException();
        }

        Project? ITodoRepo.GetPrjectFromName(string value)
        {
            throw new NotImplementedException();
        }
    }
}
