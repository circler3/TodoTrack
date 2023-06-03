using AutoMapper;
using AutoMapper.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoTrack.Contracts;

namespace TodoTrack.Cli
{
    //TODO: should be configured via interface
    public class TodoHolder
    {
        private readonly Dictionary<Type, object> _repos;
        private readonly Dictionary<Type, IEnumerable<IEntity>> _set;

        public TodoHolder(IRepo<TodoItem> todoRepo, IRepo<Project> projectRepo)
        {
            _set = new()
            {
                [GetEntityType(todoRepo)] = todoRepo.GetAsync().Result ?? throw new NullReferenceException(),
                [GetEntityType(projectRepo)] = projectRepo.GetAsync().Result ?? throw new NullReferenceException()
            };
            _repos = new()
            {
                [GetEntityType(todoRepo)] = todoRepo ?? throw new NullReferenceException(),
                [GetEntityType(projectRepo)] = projectRepo ?? throw new NullReferenceException()
            };
        }

        private static Type GetEntityType(object obj)
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

        public IEnumerable<IEntity> EntitySet<T>()
            where T : class, IEntity
        {
            if (!_set.ContainsKey(typeof(T))) throw new ArgumentException();
            return _set[typeof(T)];
        }

        public IList<T> Set<T>()
            where T : class, IEntity
        {
            if (!_set.ContainsKey(typeof(T))) throw new ArgumentException();
            return _set[typeof(T)].OfType<T>().ToList();
        }

        internal async Task<Project?> GetProjectFromNameAsync(string value)
        {
            return (await Repo<Project>().GetAsync()).SingleOrDefault(w => w.Id == value);
        }

        internal async Task AddTodayItemsAsync(IEnumerable<string> addIds)
        {
            foreach (var item in addIds)
            {
                var target = Set<TodoItem>().SingleOrDefault(w => w.Id == item);
                if (target == null) return;
                target.IsToday = true;
                target.LatestWorkTimestamp = TimestampHelper.CurrentDateStamp;
                await UpdateAsync(target);
            }
            await GetAsync<TodoItem>();
        }

        internal async Task RemoveTodayTodoItemAsync(IEnumerable<string> deleteIds)
        {
            foreach (var id in deleteIds)
            {
                var target = Set<TodoItem>().SingleOrDefault(w => id == w.Id);
                if (target == null) return;
                target.IsToday = false;
                target.IsFocus = false;
                await UpdateAsync(target);
            }
            await GetAsync<TodoItem>();
        }

        internal async Task StartTodoItemAsync(IEnumerable<string> ids)
        {
            foreach (var id in ids)
            {
                var target = Set<TodoItem>().SingleOrDefault(w => id == w.Id);
                var currentDateStamp = TimestampHelper.CurrentDateStamp;

                if (target != null)
                {
                    target.LatestWorkTimestamp = currentDateStamp;
                    target.Status = TodoStatus.InProgress;
                    target.IsToday = true;
                    target.IsFocus = true;
                    target.TodoPeriods.Add(new WorkPeriod { StartTimestamp = currentDateStamp });
                }
                else
                    Console.WriteLine("Invalid index. Cannot start Todo item");
            }
            await GetAsync<TodoItem>();
        }

        internal async Task FinishTodoItemAsync(IEnumerable<string> ids)
        {
            foreach (var id in ids)
            {
                var target = Set<TodoItem>().SingleOrDefault(w => id == w.Id);
                if (target != null)
                {
                    var currentDateStamp = TimestampHelper.CurrentDateStamp;
                    target.LatestWorkTimestamp = currentDateStamp;
                    if (target.TodoPeriods.Count != 0)
                        if (target.TodoPeriods[^1].Started) target.TodoPeriods[^1].EndTimestamp = currentDateStamp;
                    target.FinishedTimestamp = currentDateStamp;
                    //todo: recheck
                    if (currentDateStamp <= target.ScheduledDueTimestamp) target.Status = TodoStatus.FinishedOnTime;
                    else target.Status = TodoStatus.FinishedDelayed;
                    target.IsToday = false;
                    target.IsFocus = false;
                    await RemoveTodayTodoItemAsync(new[] { target.Id });
                }
                else
                    Console.WriteLine("Invalid index. Cannot start Todo item");
            }
            await GetAsync<TodoItem>();
        }

        internal async Task StopTodoItemAsync(IEnumerable<string> ids)
        {
            foreach (var id in ids)
            {
                var target = Set<TodoItem>().SingleOrDefault(w => w.Id == id);
                if (target != null)
                {
                    var currentDateStamp = TimestampHelper.CurrentDateStamp;
                    target.LatestWorkTimestamp = currentDateStamp;
                    if (target.TodoPeriods.Count == 0) return;
                    if (target.TodoPeriods[^1].Started) target.TodoPeriods[^1].EndTimestamp = currentDateStamp;
                    target.IsFocus = false;
                }
                else
                    Console.WriteLine("Invalid index. Cannot start Todo item");
            }
            await GetAsync<TodoItem>();
        }

        internal async Task<TEntity> CreateAsync<TEntity>(TEntity item)
            where TEntity : class, IEntity
        {
            var result = await Repo<TEntity>().CreateAsync(item);
            await GetAsync<TEntity>();
            return result;
        }

        internal async Task<IQueryable<TEntity>> GetAsync<TEntity>()
    where TEntity : class, IEntity
        {
            var result = await Repo<TEntity>().GetAsync();
            _set[typeof(TEntity)] = result;
            return result;
        }
        internal async Task<TEntity?> UpdateAsync<TEntity>(TEntity entity)
    where TEntity : class, IEntity
        {
            return await Repo<TEntity>().UpdateAsync(entity.Id, entity);
        }

        internal async Task DeleteAsync<TEntity>(IEnumerable<string> deleteIds)
    where TEntity : class, IEntity
        {
            foreach (var item in deleteIds)
            {
                await Repo<TEntity>().DeleteAsync(item);
            }
        }
    }
}
