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
    public class NewProjectCommand : AsyncCommand<RangeSettings>
    {
        private readonly TodoHolder _todoHolder;

        public NewProjectCommand(TodoHolder todoHolder)
        {
            _todoHolder = todoHolder;
        }
        public override async Task<int> ExecuteAsync(CommandContext context, RangeSettings settings)
        {
            string input = settings.RangeString;
            string pattern = @"(^|[$\-#%*/&])(\S+)";

            try
            {
                Project item = new();
                MatchCollection matches = Regex.Matches(input, pattern);

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
                            //parent project name
                            item.Parent = await _todoHolder.GetProjectFromNameAsync(value);
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
                            break;
                        case "-":
                            //additional command
                            break;
                        case "*":
                            //undefined
                            break;

                        default:
                            break;
                    }
                }
                if (string.IsNullOrWhiteSpace(item.Name)) return -1;
                await _todoHolder.CreateAsync(item);
                //item.CreatedTimestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
                //    item.ScheduledBeginTimestamp = TimestampHelper.CurrentDateStamp;
                //    item.Status = TodoStatus.InProgress;
                //var todo = await _todoHolder.CreateTodoItemAsync(item);
                //if (instantFlag) await _todoHolder.SetFocusAsync(todo.Id);
            }
            catch (Exception e)
            {
                AnsiConsole.WriteException(e);
                throw;
            }
            TableOutputHelper.BuildTable((await _todoHolder.GetTodoItemsAsync()));
            return 0;
        }
    }
}