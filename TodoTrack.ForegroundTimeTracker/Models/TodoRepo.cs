using ForegroundTimeTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoTrack.Contracts;

namespace TodoTrack.ForegroundTimeTracker.Models
{
    public class TodoRepo : ITodoRepo
    {
        public async ValueTask<TodoItem> GetCurrentTodoItemAsync()
        {
            return new TodoItem { Name = "Test" };
        }

        public async ValueTask<IList<TodoItem>> GetTodayTodoItemsAsync()
        {
            return new[] { new TodoItem { Name = "Test1" },
                new TodoItem { Name = "Test2" },
                 new TodoItem { Name = "Test3" },
                  new TodoItem { Name = "Test4" },
                   new TodoItem { Name = "Test5" },
                   new TodoItem { Name = "Test6" }
            };
        }

        public bool UpdateAsync(TodoItem item)
        {
            return true;
        }
    }
}
