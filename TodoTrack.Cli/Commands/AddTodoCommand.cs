using Spectre.Console;
using Spectre.Console.Cli;

namespace TodoTrack.Cli.Commands
{

    /// <summary>
    /// add a todo item to schedule today from system.
    /// </summary>
    public class AddTodoCommand : AsyncCommand<MethodSettings>
    {
        private readonly TodoHolder _todoHolder;

        public AddTodoCommand(TodoHolder todoHolder)
        {
            _todoHolder = todoHolder;
        }

        public override async Task<int> ExecuteAsync(CommandContext context, MethodSettings settings)
        {
            try
            {
                List<string> strList = RangeHelper.GetMatchedStringList(settings.Project, _todoHolder.GetTodoItemsAsync());
                await _todoHolder.AddTodayItemsAsync(strList);
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