using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForegroundTimeTracker.Models
{
    public interface ITodoRepo
    {
        IList<TodoItem> GetTodayTodoItemsAsync();
        TodoItem GetCurrentTodoItemAsync();
        bool UpdateAsync(TodoItem item);
    }
}
