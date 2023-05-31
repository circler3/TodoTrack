using Spectre.Console;
using System.Diagnostics.CodeAnalysis;

namespace TodoTrack.Cli.Commands
{
    /// <summary>
    /// add a todo item to schedule today from system.
    /// </summary>
    public class AddCommand : ITodoCommand
    {
        private readonly TodoHolder _todoHolder;

        public AddCommand(TodoHolder todoHolder)
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
            TableOutputHelper.BuildTable(_todoHolder.TodoItems, "Todo All");
            return 0;


        }
    }
}