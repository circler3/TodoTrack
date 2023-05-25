using ForegroundTimeTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoTImeTrack.ForegroundTimeTracker.Models
{
    public class WorkFromProcessRepo : IWorkFromProcessRepo
    {
        public async Task<bool> PostNewEntriesAsync(IEnumerable<WorkFromProcess> workFromProcesses)
        {
            throw new NotImplementedException();
        }
    }
}
