using Spectre.Console.Cli;

namespace TodoTrack.Cli.Commands
{
    public class RangeSettings : CategorySettings
    {
        [CommandArgument(0, "[Range]")]
        public string RangeString { get; set; } = "";
    }
}