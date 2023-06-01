using Spectre.Console;
using Spectre.Console.Cli;
using System.Diagnostics.CodeAnalysis;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace TodoTrack.Cli.Commands
{
    public class AddSettings : CommandSettings
    {
        [CommandArgument(0, "[PROJECT]")]
        public string Project { get; set; }
    }

    public class AddTodoSettings : AddSettings
    {
        [CommandArgument(0, "<BUILD_STRING>")]
        public string BuildString { get; set; }
    }
    /// <summary>
    /// add a todo item to schedule today from system.
    /// </summary>
    public class AddTodoCommand : AsyncCommand<AddTodoSettings>
    {
        private readonly TodoHolder _todoHolder;

        public AddTodoCommand(TodoHolder todoHolder)
        {
            _todoHolder = todoHolder;
        }
        public async Task<int> ExecuteAsync([NotNull] string command)
        {
            try
            {
                List<string> strList = RangeHelper.GetMatchedStringList(command, _todoHolder.TodoItems);
                await _todoHolder.AddTodayItemsAsync(strList);
            }
            catch (Exception e)
            {
                AnsiConsole.WriteException(e);
                throw;
            }
            TableOutputHelper.BuildTable(_todoHolder.TodoItems.Where(w=>w.IsToday).ToList(), "Todo Today");
            return 0;


        }

        public override async Task<int> ExecuteAsync(CommandContext context, AddTodoSettings settings)
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