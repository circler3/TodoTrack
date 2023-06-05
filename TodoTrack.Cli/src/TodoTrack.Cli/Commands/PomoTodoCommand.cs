using Spectre.Console;
using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using TodoTrack.Contracts;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace TodoTrack.Cli.Commands
{
    /// <summary>
    /// add a todo item to schedule today from system.
    /// </summary>
    public class PodoTodoCommand : AsyncCommand<PodoTodoSettings>
    {
        private readonly TodoHolder _todoHolder;

        public PodoTodoCommand(TodoHolder todoHolder)
        {
            _todoHolder = todoHolder;
        }

        public override async Task<int> ExecuteAsync(CommandContext context, PodoTodoSettings settings)
        {
            try
            {
                if(settings.Stop ?? false) 
                {
                    await _todoHolder.StopTodoItemAsync();
                    await _todoHolder.StopPomodoroAsync();
                    return 0;
                }
                if (settings.IndexString != null)
                {
                    List<string> strList = RangeHelper.GetMatchedStringList(new string[] { settings.IndexString }, _todoHolder.EntitySet<TodoItem>());
                    await _todoHolder.StartTodoItemAsync(strList[0]);
                    await _todoHolder.StartPomodoroAsync(strList[0]);
                }
            }
            catch (Exception e)
            {
                AnsiConsole.WriteException(e);
                throw;
            }
            TableOutputHelper.BuildTodoTable(_todoHolder.Set<TodoItem>().Where(w => w.IsToday).ToList(), "Todo Today");
            return 0;
        }
    }
}