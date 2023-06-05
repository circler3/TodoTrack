using Spectre.Console.Cli;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using TodoTrack.Contracts;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace TodoTrack.Cli.Commands
{
    /// <summary>
    /// list all/today todo items from system.
    /// </summary>
    public class ListProjectCommand : AsyncCommand<ListSettings>
    {
        private readonly TodoHolder _todoHolder;

        public ListProjectCommand(TodoHolder todoHolder)
        {
            _todoHolder = todoHolder;
        }

        public override async Task<int> ExecuteAsync(CommandContext context, ListSettings settings)
        {
            string input = settings.ListString ?? "";
            switch (input.ToLower())
            {
                case "today":
                    TableOutputHelper.BuildTable(_todoHolder.Set<Project>(), "Project All");
                    break;
                case "all":
                default:
                    TableOutputHelper.BuildTable(_todoHolder.Set<Project>(), "Project All");
                    break;
            }
            return await Task.FromResult(0);
        }
    }
}