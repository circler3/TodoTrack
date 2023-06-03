using System.ComponentModel.DataAnnotations;

namespace TodoTrack.Contracts
{
    public interface IEntity
    {
        [Key]
        string Id { get; set; }
        string Name { get; set; }
    }
}