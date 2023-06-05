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
    public class NewProjectCommand : AsyncCommand<NewSettings>
    {
        private readonly TodoHolder _todoHolder;

        public NewProjectCommand(TodoHolder todoHolder)
        {
            _todoHolder = todoHolder;
        }
        public override async Task<int> ExecuteAsync(CommandContext context, NewSettings settings)
        {

            try
            {
                Project item = new()
                {
                    //name
                    Name = settings.Name
                };
                if (settings.Project != null) item.Parent = _todoHolder.GetFromIndexOrName<Project>(settings.Project);
                await _todoHolder.CreateAsync(item);
            }
            catch (Exception e)
            {
                AnsiConsole.WriteException(e);
                throw;
            }
            TableOutputHelper.BuildProjectTable((await _todoHolder.GetAsync<Project>()).ToList(), "Project List");
            return 0;
        }
    }
}