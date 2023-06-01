using Spectre.Console.Cli;

namespace TodoTrack.Cli.Commands
{
    public class MethodSettings : CommandSettings
    {
        [CommandArgument(0, "[PROJECT]")]
        public string? Project { get; set; } = "";
    }
}