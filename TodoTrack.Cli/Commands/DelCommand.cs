using Spectre.Console;
using Spectre.Console.Cli;
using System.Diagnostics.CodeAnalysis;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace TodoTrack.Cli.Commands
{

    /// <summary>
    /// delete a todo item from system.
    /// </summary>
    public class DelCommand<T> : AsyncCommand<RangeSettings>
    {
        private readonly TodoHolder _todoHolder;

        public DelCommand(TodoHolder todoHolder)
        {
            _todoHolder = todoHolder;
        }

        public override async Task<int> ExecuteAsync(CommandContext context, RangeSettings settings)
        {
            try
            {
                List<string> strList = RangeHelper.GetMatchedStringList(settings.RangeString, _todoHolder.TodoItems);
                await _todoHolder.DeleteTodoItemAsync(strList);
            }
            catch (Exception e)
            {
                AnsiConsole.WriteException(e);
                throw;
            }
            TableOutputHelper.BuildTable(_todoHolder.TodoItems, "Todo All");
            return 0;
        }
    }
}