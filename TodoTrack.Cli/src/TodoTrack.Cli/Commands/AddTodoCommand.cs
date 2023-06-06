using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;
using Spectre.Console.Cli;
using TodoTrack.Contracts;

namespace TodoTrack.Cli.Commands
{

    /// <summary>
    /// add a todo item to schedule today from system.
    /// </summary>
    public class AddTodoCommand : AsyncCommand<TodoIndexSettings>
    {
        private readonly TodoHolder _todoHolder;

        //private readonly IServiceScopeFactory _scopeFactory;

        public AddTodoCommand(TodoHolder todoHolder)
        {
            _todoHolder = todoHolder;
            //_scopeFactory = scopeFactory;
        }

        public override async Task<int> ExecuteAsync(CommandContext context, TodoIndexSettings settings)
        {
            try
            {
                //using var scope = _scopeFactory.CreateScope();
                //var _todoHolder = scope.ServiceProvider.GetRequiredService<TodoHolder>();
                List<string> strList = RangeHelper.GetMatchedStringList(settings.IndexString, _todoHolder.EntitySet<TodoItem>());
                await _todoHolder.AddTodayItemsAsync(strList);
            }
            catch (Exception e)
            {
                AnsiConsole.WriteException(e);
                throw;
            }
            TableOutputHelper.BuildTodoTable(_todoHolder.Set<TodoItem>(), "Todo Today");
            return 0;
        }
    }
}