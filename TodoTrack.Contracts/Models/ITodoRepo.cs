using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoTrack.Contracts
{
    public interface ITodoRepo
    {
        Task<IList<TodoItem>> GetTodayTodoItemsAsync();
        bool UpdateAsync(TodoItem item);
        Task<Project?> GetProjectFromNameAsync(string value);
        Task<TodoItem> CreateTodoItemAsync(TodoItem item);
    }
}
