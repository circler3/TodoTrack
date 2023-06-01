using Spectre.Console;
using Spectre.Console.Cli;
using System.Diagnostics.CodeAnalysis;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace TodoTrack.Cli.Commands
{
    /// <summary>
    /// add a todo item to schedule today from system.
    /// </summary>
    public class FinishTodoCommand : AsyncCommand<MethodSettings>
    {
        private readonly TodoHolder _todoHolder;

        public FinishTodoCommand(TodoHolder todoHolder)
        {
            _todoHolder = todoHolder;
        }

        public override async Task<int> ExecuteAsync(CommandContext context, MethodSettings settings)
        {
            try
            {
                List<string> strList = RangeHelper.GetMatchedStringList(settings.Project, await _todoHolder.GetTodoItemsAsync());
                await _todoHolder.FinishTodoItemAsync(strList);
            }
            catch (Exception e)
            {
                AnsiConsole.WriteException(e);
                throw;
            }
            TableOutputHelper.BuildTable((await _todoHolder.GetTodoItemsAsync()).Where(w => w.IsToday).ToList(), "Todo Today");
            return 0;
        }
    }
}