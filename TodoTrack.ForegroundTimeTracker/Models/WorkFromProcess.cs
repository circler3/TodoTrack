using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace ForegroundTimeTracker.Models
{
    public class WorkFromProcess
    {
        //TODO: USE ABP ID generator to produce predictable Ids.
        [Key]
        public string Id { get; init; } = Guid.NewGuid().ToString();
        [NotMapped]
        public int ProcessId { get; init; }
        public string ProcessName { get; init; }
        public string Title { get; init; }
        public long StartTimestamp { get; set; } 
        public long EndTimestamp { get; set; }
        [NotMapped]
        public TimeSpan Duration => TimeSpan.FromSeconds(EndTimestamp - StartTimestamp);
        //TODO: Friendly to ORM.
        public List<long> IdlePeriods { get; init; }
        protected WorkFromProcess()
        {

        }
        public WorkFromProcess(Process process)
        {
            ProcessId = process.Id;
            ProcessName = process.ProcessName;
            Title = process.MainWindowTitle;
            StartTimestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
            EndTimestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
            IdlePeriods = new List<long>();
        }

        public WorkFromProcess(WorkFromProcess process)
        {
            ProcessId = process.ProcessId;
            ProcessName = process.ProcessName;
            Title = process.Title;
            StartTimestamp = process.StartTimestamp;
            EndTimestamp = process.EndTimestamp;
            IdlePeriods = new List<long>();
        }
    }
}
