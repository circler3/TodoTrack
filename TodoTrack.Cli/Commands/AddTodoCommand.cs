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
    public class AddTodoCommand : ICommand
    {
        private readonly TodoHolder _todoHolder;

        public AddTodoCommand(TodoHolder todoHolder)
        {
            _todoHolder = todoHolder;
        }
        public async Task<int> ExecuteAsync([NotNull] string command)
        {
            string input = command;
            string pattern = @"(^|[$\-#%*/&])(\S+)";

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
            item.CreatedTimestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
            if (instantFlag)
            {
                item.ScheduledBeginTimestamp = TimestampHelper.CurrentDateStamp;
                item.Status = TodoStatus.InProgress;
            }
            var todo = await _todoHolder.CreateTodoItemAsync(item);
            if (instantFlag) _todoHolder.SetFocus(todo);
            BuildTable(_todoHolder.TodoItems);
            return 0;
        }

        private void BuildTable(IList<IndexedTodoItem> items)
        {
            var table = new Table();

            table.AddColumn(new TableColumn("[red]Focus[/]").Centered());
            table.AddColumn("Index");
            table.AddColumn("Name");
            table.AddColumn("Status");
            table.AddColumn("Project");

            for (int i = 0; i < items.Count; i++)
            {
                List<string> sb = new()
                {
                    items[i].IsFocus ? "[red]>[/]" : "",
                    $"[green]{i}[/]",
                    items[i].Name,
                    items[i].Status.ToString(),
                    items[i].Project?.Name ?? "null"
                };
                table.AddRow(sb.ToArray());
            }

            AnsiConsole.Write(table);
        }
    }
}