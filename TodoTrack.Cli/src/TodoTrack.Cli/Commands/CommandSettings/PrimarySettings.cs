using Spectre.Console.Cli;

namespace TodoTrack.Cli.Commands
{
    public class IgnoreCategorySettings : CommandSettings
    {
        // [CommandArgument(0, "[CATEGORY]")]
        // public string? Category { get; init; }
    }

    public class TodoIndexSettings : CommandSettings
    {
        [CommandArgument(0, "<TODOITEM_INDEX>")]
        public string[] IndexString { get; init; } = default!;
    }
}