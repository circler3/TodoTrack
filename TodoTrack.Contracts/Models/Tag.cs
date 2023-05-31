using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoTrack.Contracts
{
    public class Tag : IEntity
    {
        [Key]
        public string Id { get; set; } = default!;
        public string Name { get; set; } = default!;
        [NotMapped]
        public virtual ICollection<string> MatchKeys { get; set; } = new List<string>();
    }
}