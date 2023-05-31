﻿using Spectre.Console;
using System.Diagnostics.CodeAnalysis;

namespace TodoTrack.Cli.Commands
{
    /// <summary>
    /// add a todo item to schedule today from system.
    /// </summary>
    public class FinishCommand : ITodoCommand
    {
        private readonly TodoHolder _todoHolder;

        public FinishCommand(TodoHolder todoHolder)
        {
            _todoHolder = todoHolder;
        }
        public async Task<int> ExecuteAsync([NotNull] string command)
        {
            try
            {
                List<string> strList = RangeHelper.GetMatchedStringList(command, _todoHolder.TodoItems);
                await _todoHolder.FinishTodoItemAsync(strList);
            }
            catch (Exception e)
            {
                AnsiConsole.WriteException(e);
                throw;
            }
            TableOutputHelper.BuildTable(_todoHolder.TodoItems.Where(w=>w.IsToday).ToList(), "Todo Today");
            return 0;


        }
    }
}