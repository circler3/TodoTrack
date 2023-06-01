using Spectre.Console.Cli;

namespace TodoTrack.Cli.Commands
{
    public class RangeSettings : CommandSettings
    {
        [CommandArgument(0, "<Range>")]
        public string RangeString { get; set; } =default!;
    }
}