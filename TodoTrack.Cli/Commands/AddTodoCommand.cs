using Spectre.Console.Cli;
using Spectre.Console;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using TodoTrack.Contracts;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Data;

namespace TodoTrack.Cli.Commands
{
    internal class AddTodoCommand : ICommand
    {
        private readonly ITodoRepo _todoRepo;
        public AddTodoCommand(ITodoRepo todoRepo)
        {
            _todoRepo = todoRepo;
        }
        public async Task<int> ExecuteAsync([NotNull] string command)
        {
            string input = command;
            string pattern = @"(^|[$#%*/&])(\w+)";

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
                        item.Project = _todoRepo.GetPrjectFromName(value);
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
                item.CreatedTimestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
                if (instantFlag)
                {
                    item.ScheduledBeginTimestamp = TimestampHelper.CurrentDateStamp;
                }
                var todo = await _todoRepo.CreateTodoItemAsync(item);

            }

            return 0;
        }
    }
}