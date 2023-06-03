using Spectre.Console.Cli;

namespace TodoTrack.Cli.Commands
{
    public class CategorySettings : CommandSettings
    {
        [CommandArgument(0, "[CATEGORY]")]
        public string Category { get; set; } = "";
    }

    public class TodoSettings : CommandSettings
    {
        [CommandArgument(0, "<TODOITEM_INDEX>")]
        public string IndexString { get; set; } = default!;
    }
}