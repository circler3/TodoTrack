using AutoMapper;
using AutoMapper.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoTrack.Contracts;

namespace TodoTrack.Cli
{
    //TODO: should be configured via interface
    public class TodoHolder
    {
        private readonly KeyedList<IndexedTodoItem> _todoItems;
        private string _focusId = "";
        private readonly IMapper _mapper;
        private readonly Dictionary<Type, IRepo> _repos;

        public TodoHolder(IRepo<TodoItem> todoRepo, IRepo<Project> projectRepo, IMapper mapper)
        {
            _repos = new();
            _todoItems = new();
            _repos[GetEntityType(todoRepo)] = todoRepo;
            _repos[GetEntityType(projectRepo)] = projectRepo;
            _mapper = mapper;
            int i = 0;
            _todoItems.AddRange(todoRepo.GetAsync().Result
                .OrderByDescending(w => w.ScheduledDueTimestamp).ToList().Select(w =>
                {
                    var result = mapper.Map<IndexedTodoItem>(w);
                    result.Index = i++;
                    return result;
                }));
        }

        private static Type GetEntityType(IRepo obj)
        {
            var repoInterface = obj.GetType().GetInterfaces().FirstOrDefault(w => w.IsGenericType
            && w.GetGenericTypeDefinition() == typeof(IRepo<>));
            ArgumentNullException.ThrowIfNull(repoInterface);
            return repoInterface.GenericTypeArguments[0];
        }

        private IRepo<T> Repo<T>()
            where T : class, IEntity
        {
            return (IRepo<T>)_repos[typeof(T)];
        }

        internal async Task<IList<IndexedTodoItem>> GetTodoItemsAsync()
        {
            await SetFocusAsync(_focusId);
            return _todoItems;
        }

        internal async Task<IList<Project>> GetProjectAsync()
        {
            return (await Repo<Project>().GetAsync()).ToList();
        }

        internal async Task<IList<Tag>> GetTagAsync()
        {
            return (await Repo<Tag>().GetAsync()).ToList();
        }

        internal async Task<IndexedTodoItem> CreateTodoItemAsync(TodoItem item)
        {
            var todo = await Repo<TodoItem>().CreateAsync(item);
            var iTodo = _mapper.Map<IndexedTodoItem>(todo);
            iTodo.Index = _todoItems.Count;
            _todoItems.Add(iTodo);
            return iTodo;
        }

        internal async Task<Project?> GetProjectFromNameAsync(string value)
        {
            return (await Repo<Project>().GetAsync()).SingleOrDefault(w => w.Id == value);
        }

        internal async Task SetFocusAsync(string todoId)
        {
            foreach (var item in _todoItems)
            {
                if (todoId == item.Id)
                {
                    item.IsFocus = true;
                    item.IsToday = true;
                    if (_focusId != item.Id)
                    {
                        item.LatestWorkTimestamp = TimestampHelper.CurrentDateStamp;
                        await UpdateAsync(item);
                    }
                    _focusId = item.Id;
                }
                else
                    item.IsFocus = false;
            }
        }

        internal async Task UnsetFocusAsync(string todoId)
        {
            var item = _todoItems[todoId];
            if (item == null) return;
            item.IsFocus = false;
            item.LatestWorkTimestamp = TimestampHelper.CurrentDateStamp;
            _focusId = "";
            await UpdateAsync(item);
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
                await UnsetFocusAsync(target.Id);
            }
            await Task.CompletedTask;
        }


        private async Task UpdateTodoItemAsync(IndexedTodoItem item)
        {
            await Repo<TodoItem>().UpdateAsync(item.Id, item);
        }

        internal async Task StartTodoItemAsync(IEnumerable<string> ids)
        {
            foreach (var id in ids)
            {
                var target = _todoItems[id];
                var currentDateStamp = TimestampHelper.CurrentDateStamp;

                if (target != null)
                {
                    target.LatestWorkTimestamp = currentDateStamp;
                    target.Status = TodoStatus.InProgress;
                    target.TodoPeriods.Add(new WorkPeriod { StartTimestamp = currentDateStamp });
                    await SetFocusAsync(target.Id);
                }
                else
                    Console.WriteLine("Invalid index. Cannot start Todo item");
            }
        }

        internal async Task FinishTodoItemAsync(IEnumerable<string> ids)
        {
            foreach (var id in ids)
            {
                var target = _todoItems[id];
                if (target != null)
                {
                    var currentDateStamp = TimestampHelper.CurrentDateStamp;
                    target.LatestWorkTimestamp = currentDateStamp;
                    if (target.TodoPeriods.Count == 0) return;
                    if (target.TodoPeriods[^1].Started) target.TodoPeriods[^1].EndTimestamp = currentDateStamp;
                    target.FinishedTimestamp = currentDateStamp;
                    //todo: recheck
                    if (currentDateStamp <= target.ScheduledDueTimestamp) target.Status = TodoStatus.FinishedOnTime;
                    else target.Status = TodoStatus.FinishedDelayed;
                    await UnsetFocusAsync(target.Id);
                }
                else
                    Console.WriteLine("Invalid index. Cannot start Todo item");
            }
        }

        internal async Task StopTodoItemAsync(IEnumerable<string> ids)
        {
            foreach (var id in ids)
            {
                var target = _todoItems[id];
                if (target != null)
                {
                    var currentDateStamp = TimestampHelper.CurrentDateStamp;
                    target.LatestWorkTimestamp = currentDateStamp;
                    if (target.TodoPeriods.Count == 0) return;
                    if (target.TodoPeriods[^1].Started) target.TodoPeriods[^1].EndTimestamp = currentDateStamp;
                    await UnsetFocusAsync(target.Id);
                }
                else
                    Console.WriteLine("Invalid index. Cannot start Todo item");
            }
        }

        internal async Task<TEntity> CreateAsync<TEntity>(TEntity item)
            where TEntity : class, IEntity
        {
           return await Repo<TEntity>().CreateAsync(item);
        }

        internal async Task<IQueryable<TEntity>> GetAsync<TEntity>()
    where TEntity : class, IEntity
        {
            return await Repo<TEntity>().GetAsync();
        }
        internal async Task UpdateAsync<TEntity>(TEntity entity)
    where TEntity : class, IEntity
        {
            await Repo<TEntity>().UpdateAsync(entity.Id, entity);
        }

        internal async Task DeleteAsync<TEntity>(IEnumerable<string> deleteIds)
    where TEntity : class, IEntity
        {
            foreach (var item in deleteIds)
            {
                await Repo<TEntity>().DeleteAsync(item);
                if (typeof(TEntity) == typeof(TodoItem)) _todoItems.Remove(_todoItems.Single(w => w.Id == item));
            }
        }
    }
}
