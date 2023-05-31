using Spectre.Console;
using System.Diagnostics.CodeAnalysis;

namespace TodoTrack.Cli.Commands
{
    /// <summary>
    /// delete a todo item from system.
    /// </summary>
    public class DelCommand : ITodoCommand
    {
        private readonly TodoHolder _todoHolder;

        public DelCommand(TodoHolder todoHolder)
        {
            _todoHolder = todoHolder;
        }
        public async Task<int> ExecuteAsync([NotNull] string command)
        {
            try
            {
                List<string> strList = RangeHelper.GetMatchedStringList(command, _todoHolder.TodoItems);
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