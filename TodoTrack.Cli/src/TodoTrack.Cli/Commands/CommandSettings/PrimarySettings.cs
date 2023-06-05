using System.ComponentModel;
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

    public class PodoTodoSettings : CommandSettings
    {
        [CommandArgument(0, "[TODOITEM_INDEX]")]
        [Description("Index of todoitem.")]
        public string? IndexString { get; init; }

        [CommandOption("-w|--work")]
        [Description("Work time in minutes. Default value is 45 minutes.")]
        public int? WorkTime { get; init; } = 45;

        [CommandOption("-r|--rest")]
        [Description("rest time after work in minutes. Default value is 10 minutes.")]
        public int? RestTime { get; init; } = 10;

        [CommandOption("-c|--cycle")]
        [Description("Cycles of work time and restime. Default value is 2 cycles.")]
        public int? Cycles { get; init; } = 2;

        [CommandOption("-s|--stop")]
        [Description("Stop current pomodoro work.")]
        public bool? Stop { get; init; } = false;
    }
}