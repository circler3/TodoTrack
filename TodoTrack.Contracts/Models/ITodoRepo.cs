using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoTrack.Contracts
{
    public interface ITodoRepo
    {
        ValueTask<IList<TodoItem>> GetTodayTodoItemsAsync();
        ValueTask<TodoItem> GetCurrentTodoItemAsync();
        bool UpdateAsync(TodoItem item);
    }
}
