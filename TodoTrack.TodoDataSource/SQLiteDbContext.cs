using ForegroundTimeTracker.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using TodoTrack.Contracts;

namespace TodoTrack.TodoDataSource
{
    public class SQLiteDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=d:/temp/test.db");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ProcessPeriod>().Property(x => x.IdlePeriods).HasConversion(
                v=> string.Join(',', v),
                v=> v.Split(',', StringSplitOptions.None).ToList().Select(w=> long.Parse(w)).ToList()
                );
        }
        public DbSet<ProcessPeriod> WorkFromProcesses { get; set; }
        public DbSet<TodoItem> TodoItems { get; set; }
    }
}
