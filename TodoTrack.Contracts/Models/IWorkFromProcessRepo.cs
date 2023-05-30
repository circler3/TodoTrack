using ForegroundTimeTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoTrack.Contracts
{
    public interface IWorkFromProcessRepo
    {
        Task<bool> PostNewEntriesAsync(IEnumerable<ProcessPeriod> workFromProcesses);
    }
}
