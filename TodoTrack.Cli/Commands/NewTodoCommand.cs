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
    /// add a new todo item into system.
    /// </summary>
    public class NewTodoCommand : ICommand
    {
        private readonly TodoHolder _todoHolder;

        public NewTodoCommand(TodoHolder todoHolder)
        {
            _todoHolder = todoHolder;
        }
        public async Task<int> ExecuteAsync([NotNull] string command)
        {
            string input = command;
            string pattern = @"(^|[$\-#%*/&])(\S+)";

            try
            {
                TodoItem item = new();
                MatchCollection matches = Regex.Matches(input, pattern);


                bool instantFlag = false;
                foreach (Match m in matches)
                {
                    string symbol = m.Groups[1].Value;
                    string value = m.Groups[2].Value;

                    switch (symbol)
                    {
                        case "":
                            //name
                            item.Name = value;
                            break;
                        case "$":
                            //project name
                            item.Project = await _todoHolder.GetProjectFromNameAsync(value);
                            break;
                        case "#":
                            //tag
                            item.Tags.Add(value);
                            break;
                        case "&":
                            item.MatchKeys.Add(value);
                            //match keys
                            break;
                        case "/":
                            //time
                            item.EstimatedDuration = TimestampHelper.GetDurationFromString(value);
                            break;
                        case "-":
                            //additional command
                            if (value == "s") instantFlag = true;
                            break;
                        case "*":
                            //undefined
                            break;

                        default:
                            break;
                    }
                }
                if (string.IsNullOrWhiteSpace(item.Name)) return 1;
                item.CreatedTimestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
                if (instantFlag)
                {
                    item.ScheduledBeginTimestamp = TimestampHelper.CurrentDateStamp;
                    item.Status = TodoStatus.InProgress;
                }
                var todo = await _todoHolder.CreateTodoItemAsync(item);
                if (instantFlag) _todoHolder.SetFocusAsync(todo.Id);
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