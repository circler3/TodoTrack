using Spectre.Console.Cli;

namespace TodoTrack.Cli.Commands
{
    public class MethodSettings : CommandSettings
    {
        [CommandArgument(0, "[PROJECT]")]
        public string? Project { get; set; }
    }

    public class BuildStringSettings : MethodSettings
    {
        [CommandArgument(0, "<BUILD_STRING>")]
        public string BuildString { get; set; } = default!;
    }
}