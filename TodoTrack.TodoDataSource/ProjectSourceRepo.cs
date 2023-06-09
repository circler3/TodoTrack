﻿using TodoTrack.Contracts;

namespace TodoTrack.TodoDataSource
{
    /// <summary>
    /// this is temporary use case.
    /// </summary>
    public class ProjectSourceRepo : IRepo<Project>
    {
        private readonly TodoDbContext _dbContext;

        public ProjectSourceRepo()
        {
            _dbContext = new();
            //_dbContext.Database.EnsureDeleted();
            _dbContext.Database.EnsureCreated();
        }
        //TODO: USE DTO TO MAKE ID IMUTTABLE
        public async Task<Project> CreateAsync(Project item)
        {
            item.Id = Guid.NewGuid().ToString();
            _dbContext.Projects.Add(item);
            await _dbContext.SaveChangesAsync();
            return item;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var item = _dbContext.Projects.Find(id);
            if (item == null) return false;

            _dbContext.Projects.Remove(item);
            await _dbContext.SaveChangesAsync(true);
            return true;
        }

        public async Task<IQueryable<Project>> GetAsync()
        {
            return await Task.FromResult(_dbContext.Projects);
        }

        public async Task<Project?> UpdateAsync(string id, Project item)
        {
            var target = _dbContext.Projects.Find(id);
            if (target == null) return null;
            _dbContext.Entry(target).CurrentValues.SetValues(item);
            await _dbContext.SaveChangesAsync();
            return target;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing) _dbContext?.Dispose();
        }
    }
}