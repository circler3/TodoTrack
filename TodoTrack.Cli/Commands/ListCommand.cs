using Spectre.Console.Cli;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using TodoTrack.Contracts;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace TodoTrack.Cli.Commands
{
    /// <summary>
    /// list all/today todo items from system.
    /// </summary>
    public class ListCommand : AsyncCommand<RangeSettings>
    {
        private readonly TodoHolder _todoHolder;

        public ListCommand(TodoHolder todoHolder)
        {
            _todoHolder = todoHolder;
        }

        public override async Task<int> ExecuteAsync(CommandContext context, RangeSettings settings)
        {
            switch (settings?.RangeString?.ToLower())
            {
                case "all":
                    TableOutputHelper.BuildTable((await _todoHolder.GetAsync<TodoItem>()).ToList(), "Todo All");
                    break;
                case "today":
                case "now":
                default:
                    TableOutputHelper.BuildTable((await _todoHolder.GetAsync<TodoItem>()).Where(w => w.IsToday).ToList(), "Todo Today");
                    break;
            }
            return 0;
        }
    }
}