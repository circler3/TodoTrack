using Spectre.Console;
using Spectre.Console.Cli;
using System.Diagnostics.CodeAnalysis;
using TodoTrack.Contracts;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace TodoTrack.Cli.Commands
{
    /// <summary>
    /// add a todo item to schedule today from system.
    /// </summary>
    public class StopTodoCommand : AsyncCommand<TodoSettings>
    {
        private readonly TodoHolder _todoHolder;

        public StopTodoCommand(TodoHolder todoHolder)
        {
            _todoHolder = todoHolder;
        }

        public override async Task<int> ExecuteAsync(CommandContext context, TodoSettings settings)
        {
            try
            {
                List<string> strList = RangeHelper.GetMatchedStringList(settings.IndexString, _todoHolder.Set<TodoItem>().OfType<IEntity>().ToList());
                await _todoHolder.StopTodoItemAsync(strList);
            }
            catch (Exception e)
            {
                AnsiConsole.WriteException(e);
                throw;
            }
            TableOutputHelper.BuildTable((await _todoHolder.GetAsync<TodoItem>()).Where(w => w.IsToday).ToList(), "Todo Today");
            return 0;
        }
    }
}