using Spectre.Console;
using Spectre.Console.Cli;
using System.Diagnostics.CodeAnalysis;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace TodoTrack.Cli.Commands
{


    /// <summary>
    /// add a todo item to schedule today from system.
    /// </summary>
    public class AddTodoCommand : AsyncCommand<BuildStringSettings>
    {
        private readonly TodoHolder _todoHolder;

        public AddTodoCommand(TodoHolder todoHolder)
        {
            _todoHolder = todoHolder;
        }

        public override async Task<int> ExecuteAsync(CommandContext context, BuildStringSettings settings)
        {
            try
            {
                List<string> strList = RangeHelper.GetMatchedStringList(settings.BuildString, _todoHolder.TodoItems);
                await _todoHolder.AddTodayItemsAsync(strList);
            }
            catch (Exception e)
            {
                AnsiConsole.WriteException(e);
                throw;
            }
            TableOutputHelper.BuildTable(_todoHolder.TodoItems.Where(w => w.IsToday).ToList(), "Todo Today");
            return 0;
        }
    }
}