using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoTrack.Contracts;

namespace TodoTrack.Cli
{
    public class TodoHolder
    {
        private readonly List<IndexedTodoItem> _todoItems;
        private readonly ITodoRepo _todoRepo;
        private readonly IMapper _mapper;

        public TodoHolder(ITodoRepo todoRepo, IMapper mapper)
        {
            _todoItems = new List<IndexedTodoItem>();
            _todoRepo = todoRepo;
            _mapper = mapper;
            _todoItems.AddRange(todoRepo.GetTodayTodoItemsAsync().Result
                .OrderByDescending(w=>w.ScheduledDueTimestamp).Select(w=> mapper.Map<IndexedTodoItem>(w)));
        }

        internal List<IndexedTodoItem> TodoItems => _todoItems;

        internal async Task<IndexedTodoItem> CreateTodoItemAsync(TodoItem item)
        {
            var todo = await _todoRepo.CreateTodoItemAsync(item);
            var iTodo = _mapper.Map<IndexedTodoItem>(todo);
            TodoItems.Add(iTodo);
            return iTodo;
        }

        internal async Task<Project?> GetProjectFromNameAsync(string value)
        {
            return await _todoRepo.GetProjectFromNameAsync(value);
        }

        internal void SetFocus(IndexedTodoItem todo)
        {
            foreach (var item in TodoItems)
            {
                if(todo.Id == item.Id) item.IsFocus = true;
                else item.IsFocus = false;
            }
        }
    }
}
