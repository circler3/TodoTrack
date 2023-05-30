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
    /// delelte a todo item from system.
    /// </summary>
    public class ListTodoCommand : ICommand
    {
        private readonly TodoHolder _todoHolder;

        public ListTodoCommand(TodoHolder todoHolder)
        {
            _todoHolder = todoHolder;
        }
        public async Task<int> ExecuteAsync([NotNull] string command)
        {
            switch (command.ToLower())
            {
                case "all":
                    TableOutputHelper.BuildTable(await _todoHolder.GetAllTodoListAsync());
                    break;
                case "today":
                    TableOutputHelper.BuildTable(await _todoHolder.GetTodayTodoListAsync());
                    break;
                case "now":
                default:
                    TableOutputHelper.BuildTable(await _todoHolder.GetNowTodoListAsync());
                    break;
            }
            return 0;
        }


    }
}