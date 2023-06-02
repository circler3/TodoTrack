using Spectre.Console;
using Spectre.Console.Cli;
using TodoTrack.Contracts;

namespace TodoTrack.Cli.Commands
{

    /// <summary>
    /// add a todo item to schedule today from system.
    /// </summary>
    public class AddTodoCommand : AsyncCommand<TodoSettings>
    {
        private readonly TodoHolder _todoHolder;

        public AddTodoCommand(TodoHolder todoHolder)
        {
            _todoHolder = todoHolder;
        }

        public override async Task<int> ExecuteAsync(CommandContext context, TodoSettings settings)
        {
            try
            {
                List<string> strList = RangeHelper.GetMatchedStringList(settings.IndexString, (await _todoHolder.GetAsync<TodoItem>()).OfType<IEntity>().ToList());
                await _todoHolder.AddTodayItemsAsync(strList);
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