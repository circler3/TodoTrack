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
    public class NewTodoCommand : AsyncCommand<NewSettings>
    {
        private readonly TodoHolder _todoHolder;

        public NewTodoCommand(TodoHolder todoHolder)
        {
            _todoHolder = todoHolder;
        }
        public override async Task<int> ExecuteAsync(CommandContext context, NewSettings settings)
        {
            //string[] input = settings.BuildString;
            //string pattern = @"(^|[$\-#%*/&])(\S+)";

            try
            {
                TodoItem item = new()
                {
                    Name = settings.Name
                };
                if (settings.Project != null)
                    item.Project = _todoHolder.GetFromIndexOrName<Project>(settings.Project);
                if (settings.Tags != null)
                {
                    foreach (var n in settings.Tags)
                    {
                        var tag = _todoHolder.GetFromIndexOrName<Tag>(n);
                        if(tag != null) item.Tags.Add(tag);
                    }
                }
                if (settings.Cost != null) item.EstimatedDuration = TimestampHelper.GetDurationFromString(settings.Cost);
                item.CreatedTimestamp = TimestampHelper.CurrentDateStamp;;
                if (settings.Start ?? false)
                {
                    item.ScheduledBeginTimestamp = TimestampHelper.CurrentDateStamp;
                    item.Status = TodoStatus.InProgress;
                    item.IsFocus = true;
                }
                var todo = await _todoHolder.CreateAsync(item);
            }
            catch (Exception e)
            {
                AnsiConsole.WriteException(e);
                throw;
            }
            TableOutputHelper.BuildTodoTable(_todoHolder.Set<TodoItem>());
            return 0;
        }
    }
}