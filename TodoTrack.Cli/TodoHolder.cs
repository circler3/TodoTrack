using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
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
        private readonly KeyedList<IndexedTodoItem> _todoItems;
        private string _focusId = "";
        private readonly ITodoRepo _todoRepo;
        private readonly IMapper _mapper;

        public TodoHolder(ITodoRepo todoRepo, IMapper mapper)
        {
            _todoItems = new();
            _todoRepo = todoRepo;
            _mapper = mapper;
            int i = 0;
            _todoItems.AddRange(todoRepo.GetTodayTodoItemsAsync().Result
                .OrderByDescending(w => w.ScheduledDueTimestamp).Select(w =>
                {
                    var result = mapper.Map<IndexedTodoItem>(w);
                    result.Index = i++;
                    return result;
                }));
        }
        internal List<IndexedTodoItem> TodoItems
        {
            get
            {
                SetFocusAsync(_focusId);
                return _todoItems;
            }
        }

        internal async Task<IndexedTodoItem> CreateTodoItemAsync(TodoItem item)
        {
            var todo = await _todoRepo.CreateTodoItemAsync(item);
            var iTodo = _mapper.Map<IndexedTodoItem>(todo);
            iTodo.Index = _todoItems.Count;
            _todoItems.Add(iTodo);
            return iTodo;
        }

        internal async Task<Project?> GetProjectFromNameAsync(string value)
        {
            return await _todoRepo.GetProjectFromNameAsync(value);
        }

        internal async void SetFocusAsync(string todoId)
        {
            foreach (var item in _todoItems)
            {
                if (todoId == item.Id)
                {
                    item.IsFocus = true;
                    if (_focusId != item.Id)
                    {
                        item.LatestWorkTimestamp = TimestampHelper.CurrentDateStamp;
                        await UpdateTodoItemAsync(item);
                    }
                    _focusId = item.Id;
                }
                else
                    item.IsFocus = false;
            }
        }

        internal async Task AddTodayItemsAsync(IEnumerable<string> addIds)
        {
            foreach (var item in addIds)
            {
                var target = _todoItems.SingleOrDefault(w => w.Id == item);
                if (target == null) return;
                target.IsToday = true;
            }
            await Task.CompletedTask;
        }

        internal async Task RemoveTodayTodoItemAsync(IEnumerable<string> deleteIds)
        {
            foreach (var item in deleteIds)
            {
                var target = (_todoItems.SingleOrDefault(w => w.Id == item));
                if (target == null) return;
                target.IsToday = false;
            }
            await Task.CompletedTask;
        }


        private async Task UpdateTodoItemAsync(IndexedTodoItem item)
        {
            await _todoRepo.UpdateTodoItemAsync(item.Id, item);
        }

        internal async Task DeleteTodoItemAsync(IEnumerable<string> deleteIds)
        {
            foreach (var item in deleteIds)
            {
                await _todoRepo.DeleteTodoItemAsync(item);
                _todoItems.Remove(_todoItems.Single(w => w.Id == item));
            }
        }

        internal async Task<IList<IndexedTodoItem>> GetAllTodoListAsync()
        {
            return await Task.FromResult(_todoItems);
        }

    }
}
