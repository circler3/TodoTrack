﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;
using TodoTrack.Contracts;

namespace TodoTrack.TodoDataSource
{
    public class TodoDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=d:/test.db");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ProcessPeriod>().Property(x => x.IdlePeriods).HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.None).ToList().Select(w => long.Parse(w)).ToList()
                );
            modelBuilder.Entity<TodoItem>().Property(x => x.NotifyTimestamps).HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.None).ToList().Select(w => long.Parse(w)).ToList()
                );
            modelBuilder.Entity<Tag>().Property(x=>x.MatchKeys).HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.None).ToList()
            );
            modelBuilder.Entity<TodoItem>().Property(x=>x.TodoPeriods).HasConversion(
                v => string.Join(',', WorkPeriodConverter.ConvertToString(v)),
                v => WorkPeriodConverter.ParseFromString(v));
            modelBuilder.Entity<TodoItem>().Property(x=>x.ProcessPeriods).HasConversion(
                v => string.Join(',', WorkPeriodConverter.ConvertToString(v)),
                v => WorkPeriodConverter.ParseFromString(v));
        }
        public DbSet<ProcessPeriod> ProcessPeriods { get; set; } = default!;
        public DbSet<TodoItem> TodoItems { get; set; } = default!;
        public DbSet<Project> Projects { get; set; } = default!;
        public DbSet<Tag> Tags { get; set; } = default!;
        public DbSet<Attachment> Attachments { get; set; } = default!;
    }
}
