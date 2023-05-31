using Spectre.Console.Cli;
using Spectre.Console;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using TodoTrack.Contracts;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Data;
using System.Text;
using Microsoft.Extensions.Primitives;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace TodoTrack.Cli.Commands
{
    /// <summary>
    /// remove a todo item from list of today.
    /// </summary>
    public class RemoveCommand : ITodoCommand
    {
        private readonly TodoHolder _todoHolder;

        public RemoveCommand(TodoHolder todoHolder)
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
    }
}