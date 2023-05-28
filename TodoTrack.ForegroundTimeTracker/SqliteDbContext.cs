using ForegroundTimeTracker.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace TodoTrack.ForegroundTimeTracker
{
    public class SQLiteDbContext : DbContext
    {
        public SQLiteDbContext(DbContextOptions<SQLiteDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<WorkFromProcess>().Property(x => x.IdlePeriods).HasConversion(
                v=> string.Join(',', v),
                v=> v.Split(',', StringSplitOptions.None).ToList().Select(w=> long.Parse(w)).ToList()
                );
        }
        public DbSet<WorkFromProcess> WorkFromProcesses { get; set; }

        
    }
}
