using Spectre.Console.Cli;

namespace TodoTrack.Cli.Commands
{
    public class RangeSettings : MethodSettings
    {
        [CommandArgument(0, "[Range]")]
        public string RangeString { get; set; } = "";
    }
}