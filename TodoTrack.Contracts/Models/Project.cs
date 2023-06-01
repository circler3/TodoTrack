namespace TodoTrack.Contracts
{
    public class Project : IEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = default!;
        public Project? Parent { get; set; }
    }
}