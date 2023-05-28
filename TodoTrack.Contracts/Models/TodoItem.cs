using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Http.Headers;

namespace TodoTrack.Contracts
{
    public class TodoItem
    {
        [Key]
        public virtual string Id { get; init; }
        public virtual required string Name { get; set; }
        public virtual string? Description { get; set; }
        public virtual string? Comment { get; set; }
        public virtual Project? Project { get; set; }
        public virtual IEnumerable<Category>? Categories { get; set; }
        public virtual IEnumerable<string>? Tags { get; set; }
        public virtual IEnumerable<TodoItem>? SubTodoItems { get; set; }
        public virtual long? CreatedTimestamp { get; set; }
        public virtual long? FinishedTimestamp { get; set; }
        public virtual long? ScheduledBeginTimestamp { get; set; }
        public virtual long? ScheduledDueTimestamp { get; set; }
        public virtual IList<long>? NotifyTimestamp { get; set; }
        public virtual long? EstimatedDuration { get; set; }
        public virtual string? RepeatCron { get; set; }
        public virtual bool Repeatable { get; set; } = false;
        public virtual TodoStatus Status { get; set; }
        public virtual EisenhowerMatrix Priority { get; set; }
        public virtual IEnumerable<string>? MatchKeys { get; set; }
        public virtual IEnumerable<WorkPeriod>? WorkPeriods { get; set; } 
        public virtual IEnumerable<Attachment>? Attachments { get; set; } 
    }
}