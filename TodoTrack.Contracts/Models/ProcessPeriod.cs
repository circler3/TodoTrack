using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace TodoTrack.Contracts
{
    public class ProcessPeriod : IEntity
    {
#nullable disable
        protected ProcessPeriod()
        {
            // For ORM ONLY
        }
#nullable enable
        //TODO: USE ABP ID generator to produce predictable Ids.
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        [NotMapped]
        public int ProcessId { get; init; }
        public string Title { get; init; }
        public long StartTimestamp { get; set; }
        public long EndTimestamp { get; set; }
        [NotMapped]
        public TimeSpan Duration => TimeSpan.FromSeconds(EndTimestamp - StartTimestamp);
        //TODO: Friendly to ORM.
        public List<long> IdlePeriods { get; init; }
        public ProcessPeriod(Process process)
        {
            ProcessId = process.Id;
            Name = process.ProcessName;
            Title = process.MainWindowTitle;
            StartTimestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
            EndTimestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
            IdlePeriods = new List<long>();
        }

        public ProcessPeriod(ProcessPeriod process)
        {
            ProcessId = process.ProcessId;
            Name = process.Name;
            Title = process.Title;
            StartTimestamp = process.StartTimestamp;
            EndTimestamp = process.EndTimestamp;
            IdlePeriods = new List<long>();
        }
    }
}
