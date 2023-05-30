using TodoTrack.Contracts;

namespace TodoTrack.Cli
{
    internal class IndexedTodoItem : TodoItem
    {
        public bool IsFocus { get; set; } = false;
    }
}