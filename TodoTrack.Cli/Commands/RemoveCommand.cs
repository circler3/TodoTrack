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
    public class RemoveCommand : ICommand
    {
        private readonly TodoHolder _todoHolder;

        public RemoveCommand(TodoHolder todoHolder)
        {
            _todoHolder = todoHolder;
        }
        public async Task<int> ExecuteAsync([NotNull] string command)
        {
            var todoIdToDelete = command.Split(' ');


            try
            {
                if (todoIdToDelete.Length == 1 && !int.TryParse(todoIdToDelete[0], out var _))
                {
                    var range = RangeHelper.GetRange(command);
                    if (range == null)
                    {
                        await Console.Out.WriteLineAsync("Invalid range string.");
                        return -1;
                    }
                    var t = _todoHolder.TodoItems.ToArray()[range.Value].Select(w => w.Id);
                    await _todoHolder.RemoveTodayTodoItemAsync(t);
                }
                else
                {
                    List<string> index = new();
                    foreach (var item in todoIdToDelete)
                    {
                        if (int.TryParse(item, out var result))
                        {
                            if (result >= _todoHolder.TodoItems.Count) continue;
                            index.Add(_todoHolder.TodoItems[result].Id);
                        }
                    }
                    await _todoHolder.RemoveTodayTodoItemAsync(index);
                }
            }
            catch (Exception e)
            {
                AnsiConsole.WriteException(e);
                throw;
            }
            TableOutputHelper.BuildTable(_todoHolder.TodoItems);
            return 0;
        }
    }
}