﻿using AutoMapper;
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
        private readonly List<IndexedTodoItem> _todoItems;
        private string _focusId;
        private readonly ITodoRepo _todoRepo;
        private readonly IMapper _mapper;

        public TodoHolder(ITodoRepo todoRepo, IMapper mapper)
        {
            _todoItems = new List<IndexedTodoItem>();
            _todoRepo = todoRepo;
            _mapper = mapper;
            _todoItems.AddRange(todoRepo.GetTodayTodoItemsAsync().Result
                .OrderByDescending(w => w.ScheduledDueTimestamp).Select(w => mapper.Map<IndexedTodoItem>(w)));
        }

        internal List<IndexedTodoItem> TodoItems
        {
            get
            {
                SetFocus(_focusId);
                return _todoItems;
            }
        }

        internal async Task<IndexedTodoItem> CreateTodoItemAsync(TodoItem item)
        {
            var todo = await _todoRepo.CreateTodoItemAsync(item);
            var iTodo = _mapper.Map<IndexedTodoItem>(todo);
            _todoItems.Add(iTodo);
            return iTodo;
        }

        internal async Task<Project?> GetProjectFromNameAsync(string value)
        {
            return await _todoRepo.GetProjectFromNameAsync(value);
        }

        internal void SetFocus(string todoId)
        {
            foreach (var item in _todoItems)
            {
                if (todoId == item.Id)
                {
                    item.IsFocus = true;
                    _focusId = item.Id;
                }
                else
                    item.IsFocus = false;
            }
        }

        internal async void DeleteTodoItemAsync(IList<int> deleteIds)
        {
            var sorted = deleteIds.OrderDescending();
            for (var i = deleteIds.Count - 1; i >= 0; i--)
            {
                if (_todoItems.Count <= deleteIds[i]) continue;
                await _todoRepo.DeleteTodoItemAsync(_todoItems[deleteIds[i]].Id);
                _todoItems.RemoveAt(deleteIds[i]);
            }
        }

        internal async Task<IList<IndexedTodoItem>> GetAllTodoListAsync()
        {
            return _mapper.Map<IList<IndexedTodoItem>>(await _todoRepo.GetTodayTodoItemsAsync());
        }

        internal async Task<IList<IndexedTodoItem>> GetTodayTodoListAsync()
        {
            return _mapper.Map<IList<IndexedTodoItem>>(await _todoRepo.GetTodayTodoItemsAsync());
        }

        internal async Task<IList<IndexedTodoItem>> GetNowTodoListAsync()
        {
            return TodoItems;
        }
    }
}