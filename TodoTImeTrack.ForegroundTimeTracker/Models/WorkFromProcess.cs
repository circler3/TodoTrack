using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace ForegroundTimeTracker.Models
{
    public class WorkFromProcess
    {
        [Key]
        public string Id { get; init; }
        [NotMapped]
        public int ProcessId { get; init; }
        public string ProcessName { get; init; }
        public string Title { get; init; }
        public DateTimeOffset StartTime { get; init; }
        public DateTimeOffset EndTime { get; set; }
        [NotMapped]
        public TimeSpan Duration => EndTime - StartTime;
        //TODO: Friendly to ORM.
        public List<DateTimeOffset> IdlePeriods { get; init; }
        protected WorkFromProcess()
        {

        }
        public WorkFromProcess(Process process)
        {
            ProcessId = process.Id;
            ProcessName = process.ProcessName;
            Title = process.MainWindowTitle;
            StartTime = DateTimeOffset.Now;
            IdlePeriods = new List<DateTimeOffset>();
        }
    }
}
