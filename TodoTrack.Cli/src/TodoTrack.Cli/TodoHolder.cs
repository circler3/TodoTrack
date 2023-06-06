using AutoMapper;
using AutoMapper.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoTrack.Contracts;

namespace TodoTrack.Cli
{
    //TODO: should be configured via interface and be divided into several smaller services
    // this service is scoped
    public class TodoHolder 
    {
        //private readonly Dictionary<Type, object> _repos;
        private readonly Dictionary<Type, IEnumerable<IEntity>> _set;
        private readonly IServiceScopeFactory _scopeFactory;

        public TodoHolder(IServiceScopeFactory scopeFactory, IRepo<TodoItem> todoRepo, IRepo<Project> projectRepo, IRepo<Tag> tagRepo)
        {
            _set = new()
            {
                [GetEntityType(todoRepo)] = todoRepo.GetAsync().Result.ToList() ?? throw new NullReferenceException(),
                [GetEntityType(projectRepo)] = projectRepo.GetAsync().Result.ToList() ?? throw new NullReferenceException(),
                [GetEntityType(tagRepo)] = tagRepo.GetAsync().Result.ToList() ?? throw new NullReferenceException()
            };
            //_repos = new()
            //{
            //    [GetEntityType(todoRepo)] = todoRepo ?? throw new NullReferenceException(),
            //    [GetEntityType(projectRepo)] = projectRepo ?? throw new NullReferenceException(),
            //    [GetEntityType(tagRepo)] = tagRepo ?? throw new NullReferenceException()
            //};
            _scopeFactory = scopeFactory;
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
            return _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<IRepo<T>>();
            //return (IRepo<T>)_repos[typeof(T)];
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


        internal T? GetFromIndexOrName<T>(string indexOrName)
            where T : class, IEntity
        {
            if(int.TryParse(indexOrName, out var index))
            {
                return Set<T>()[index];
            }
            else
            {
                return Set<T>().FirstOrDefault(w=> w.Name == indexOrName);
            }
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

        internal async Task StartTodoItemAsync(string id)
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
            await GetAsync<TodoItem>();
        }

        internal async Task StartPomodoroAsync(string id)
        {
            //currently podo is a client only procedure.
        }

        internal async Task StopPomodoroAsync()
        {
            //currently podo is a client only procedure.
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

        internal async Task StopTodoItemAsync()
        {
            var target = Set<TodoItem>().Where(w => w.IsFocus).OrderByDescending(w => w.LatestWorkTimestamp).FirstOrDefault();
            if (target == null)
            {
                Console.WriteLine("Incorrect status");
                return;
            }
            await StopTodoItemAsync(new[] { target.Id });
        }
        internal async Task<TEntity> CreateAsync<TEntity>(TEntity item)
            where TEntity : class, IEntity
        {
            using var repo = Repo<TEntity>();
            var result = await repo.CreateAsync(item);
            var res = await repo.GetAsync();
            _set[typeof(TEntity)] = res.ToList();
            return result;
        }

        internal async Task<IQueryable<TEntity>> GetAsync<TEntity>()
    where TEntity : class, IEntity
        {
            using var repo = Repo<TEntity>();
            var result = await repo.GetAsync();
            var p = result.ToList();
            _set[typeof(TEntity)] = p;
            return p.AsQueryable();
        }
        internal async Task<TEntity?> UpdateAsync<TEntity>(TEntity entity)
    where TEntity : class, IEntity
        {
            using var repo = Repo<TEntity>();
            return await repo.UpdateAsync(entity.Id, entity);
        }

        internal async Task DeleteAsync<TEntity>(IEnumerable<string> deleteIds)
    where TEntity : class, IEntity
        {
            using var repo = Repo<TEntity>();
            foreach (var item in deleteIds)
            {
                await repo.DeleteAsync(item);
            }
        }

    }
}
