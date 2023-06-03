namespace TodoTrack.Contracts
{
    public class Attachment : IEntity
    {
        public string Id { get; set; } = default!;
        public string Name { get; set; } = default!;
        public virtual TodoItem? TodoItem {get;set;}
    }
}