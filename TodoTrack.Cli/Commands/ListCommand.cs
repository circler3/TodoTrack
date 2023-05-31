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

namespace TodoTrack.Cli.Commands
{
    /// <summary>
    /// list all/today todo items from system.
    /// </summary>
    public class ListCommand : ICommand
    {
        private readonly TodoHolder _todoHolder;

        public ListCommand(TodoHolder todoHolder)
        {
            _todoHolder = todoHolder;
        }
        public async Task<int> ExecuteAsync([NotNull] string command)
        {
            switch (command.ToLower())
            {
                case "all":
                    TableOutputHelper.BuildTable(await _todoHolder.GetAllTodoListAsync(), "Todo All");
                    break;
                case "today":
                case "now":
                default:
                    TableOutputHelper.BuildTable(_todoHolder.TodoItems.Where(w => w.IsToday).ToList(), "Todo Today");
                    break;
            }
            return 0;
        }


    }
}