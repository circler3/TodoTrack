using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoTrack.Contracts
{
    [Index("Name", IsUnique = true)]
    public class Tag : IEntity
    {
        [Key]
        public string Id { get; set; } = default!;
        public string Name { get; set; } = default!;
        public virtual ICollection<TodoItem> TodoItems { get; set; } = new List<TodoItem>();
        public virtual ICollection<string> MatchKeys { get; set; } = new List<string>();
    }
}