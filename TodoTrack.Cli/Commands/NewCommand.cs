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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace TodoTrack.Cli.Commands
{
    /// <summary>
    /// add a new todo item into system.
    /// </summary>
    public class NewCommand : AsyncCommand<RangeSettings>
    {
        private readonly TodoHolder _todoHolder;

        public NewCommand(TodoHolder todoHolder)
        {
            _todoHolder = todoHolder;
        }
        public override async Task<int> ExecuteAsync(CommandContext context, RangeSettings settings)
        {
            string input = settings.RangeString;
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
                            //TODO: Implement tags
                            //item.Tags.Add(value);
                            break;
                        case "&":
                            //match keys of tags

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
                if (string.IsNullOrWhiteSpace(item.Name)) return -1;
                item.CreatedTimestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
                if (instantFlag)
                {
                    item.ScheduledBeginTimestamp = TimestampHelper.CurrentDateStamp;
                    item.Status = TodoStatus.InProgress;
                }
                var todo = await _todoHolder.CreateTodoItemAsync(item);
                if (instantFlag) await _todoHolder.SetFocusAsync(todo.Id);
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