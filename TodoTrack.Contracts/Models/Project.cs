namespace TodoTrack.Contracts
{
    public class Project
    {
        public required string Id { get; set; }
        public string Name { get; init; } = default!;
    }
}