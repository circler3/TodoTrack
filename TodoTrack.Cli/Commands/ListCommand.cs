using System.Data;
using System.Diagnostics.CodeAnalysis;

namespace TodoTrack.Cli.Commands
{
    /// <summary>
    /// list all/today todo items from system.
    /// </summary>
    public class ListCommand : ITodoCommand
    {
        private readonly TodoHolder _todoHolder;

        public ListCommand(TodoHolder todoHolder)
        {
            _todoHolder = todoHolder;
        }
        public async Task<int> ExecuteAsync([NotNull] string command)
        {
            switch (command.ToLower())
            {
                case "all":
                    TableOutputHelper.BuildTable(await _todoHolder.GetAllTodoListAsync(), "Todo All");
                    break;
                case "today":
                case "now":
                default:
                    TableOutputHelper.BuildTable(_todoHolder.TodoItems.Where(w => w.IsToday).ToList(), "Todo Today");
                    break;
            }
            return 0;
        }


    }
}