using ForegroundTimeTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoTrack.ForegroundTimeTracker;

namespace TodoTImeTrack.ForegroundTimeTracker.Models
{
    public class WorkFromProcessRepo : IWorkFromProcessRepo
    {
        private readonly SqliteDbContext _context;

        public WorkFromProcessRepo(SqliteDbContext context)
        {
            _context = context;
        }
        public async Task<bool> PostNewEntriesAsync(IEnumerable<WorkFromProcess> workFromProcesses)
        {
            await _context.Database.EnsureCreatedAsync();
            await _context.WorkFromProcesses.AddRangeAsync(workFromProcesses);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
