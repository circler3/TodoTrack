using Spectre.Console.Cli;

namespace TodoTrack.Cli.Commands
{
    public class NewSettings : IgnoreCategorySettings
    {
        [CommandArgument(0, "<BUILD_STRING>")]
        public string Name { get; init; } = default!;
        [CommandOption("-p|--project")]
        public string? Project { get; init; }
        [CommandOption("-t|--tag")]
        public string[]? Tags { get; init; }
        [CommandOption("-c|--cost")]
        public string? Cost { get; set; }
        [CommandOption("-s|--start")]
        public bool? Start { get; init; }
    }

    public class ListSettings : IgnoreCategorySettings
    {
        [CommandArgument(0, "[LIST_STRING]")]
        public string? ListString { get; init; } = default!;
    }

        public class DelSettings : IgnoreCategorySettings
    {
        [CommandArgument(0, "<RANGE>")]
        public string[] ListString { get; init; } = default!;
    }
}