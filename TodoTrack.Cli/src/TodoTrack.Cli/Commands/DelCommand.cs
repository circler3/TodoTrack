using Spectre.Console;
using Spectre.Console.Cli;
using System.Diagnostics.CodeAnalysis;
using TodoTrack.Contracts;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace TodoTrack.Cli.Commands
{

    /// <summary>
    /// delete a todo item from system.
    /// </summary>
    public class DelCommand<T> : AsyncCommand<RangeSettings>
        where T : class, IEntity
    {
        private readonly TodoHolder _todoHolder;

        public DelCommand(TodoHolder todoHolder)
        {
            _todoHolder = todoHolder;
        }

        public override async Task<int> ExecuteAsync(CommandContext context, RangeSettings settings)
        {
            try
            {
                List<string> strList = RangeHelper.GetMatchedStringList(settings.RangeString ?? settings.Category, _todoHolder.EntitySet<T>());
                await _todoHolder.DeleteAsync<T>(strList);
            }
            catch (Exception e)
            {
                AnsiConsole.WriteException(e);
                throw;
            }
            TableOutputHelper.BuildTable<T>(_todoHolder.Set<T>(), "Todo All");
            return 0;
        }
    }
}