using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Http.Headers;

namespace ForegroundTimeTracker.Models
{
    public class TodoItem
    {
        [Key]
        public virtual string Id { get; init; }
        public virtual required string Name { get; set; }
        public virtual string? Description { get; set; }
        public virtual Project? Project { get; set; }
        public virtual IEnumerable<Category>? Categories { get; set; }
        public virtual IEnumerable<string>? Tags { get; set; }
        [NotMapped]
        public virtual IEnumerable<string>? ProcessMatchKeys { get; set; }
    }
}