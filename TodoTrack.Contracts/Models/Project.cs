namespace TodoTrack.Contracts
{
    public class Project : IEntity
    {
        public required string Id { get; set; }
        public string Name { get; set; } = default!;
    }
}