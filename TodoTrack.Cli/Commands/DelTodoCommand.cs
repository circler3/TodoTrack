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
    /// delelte a todo item from system.
    /// </summary>
    public class DelTodoCommand : ICommand
    {
        private readonly TodoHolder _todoHolder;

        public DelTodoCommand(TodoHolder todoHolder)
        {
            _todoHolder = todoHolder;
        }
        public async Task<int> ExecuteAsync([NotNull] string command)
        {
            var todoIdToDelete = command.Split(' ');


            try
            {
                List<int> index = new();
                if (todoIdToDelete.Length == 1 && !int.TryParse(todoIdToDelete[0], out var _))
                {
                    var range = GetRange(command);
                    if (range == null)
                    {
                        await Console.Out.WriteLineAsync("Invalid range string.");
                        return -1;
                    }
                    for (int i = range.Value.Start.Value; i < range.Value.End.Value; i++) index.Add(i);
                }
                else
                {
                    foreach (var item in todoIdToDelete)
                    {
                        if (int.TryParse(item, out var result))
                        {
                            index.Add(result);
                        }
                    }
                }
                _todoHolder.DeleteTodoItemAsync(index);
            }
            catch (Exception e)
            {
                AnsiConsole.WriteException(e);
                throw;
            }
            TableOutputHelper.BuildTable(_todoHolder.TodoItems);
            return 0;
        }

        private static Range? GetRange(string rangeString)
        {
            // Create the script options with the references and imports needed
            ScriptOptions options = ScriptOptions.Default
                .WithReferences(typeof(Range).Assembly)
                .WithImports("System");

            Range? myRange = null;
            try
            {
                // Evaluate the range expression string as a Range object
                myRange = CSharpScript.EvaluateAsync<Range>(
                        "Range p=" + rangeString + ";return p;", options).Result;
            }
            catch (CompilationErrorException)
            {

            }


            return myRange;
        }
    }
}