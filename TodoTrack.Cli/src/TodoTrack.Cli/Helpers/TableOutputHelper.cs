using System.Collections.Generic;
using Spectre.Console;
using TodoTrack.Contracts;

namespace TodoTrack.Cli
{
    internal class TableOutputHelper
    {
        public static void BuildTodoTable(IList<TodoItem> items, string title = "")
        {
            var table = new Table();
            if (!string.IsNullOrWhiteSpace(title)) table.Title = new TableTitle(title.Trim());

            table.AddColumn(new TableColumn("[red]Focus[/]").Centered());
            table.AddColumn("Index");
            table.AddColumn("Name");
            table.AddColumn("Status");
            table.AddColumn("Project");

            for (int i = 0; i < items.Count; i++)
            {
                string color = "purple";
                if (items[i].IsToday) color = "white";
                List<string> sb = new()
                    {
                        items[i].IsFocus ? "[red]>[/]" : "",
                        $"[green]{i}[/]",
                        $"[{color}]{items[i].Name}[/]",
                        $"[{color}]{items[i].Status}[/]",
                        $"[{color}]{items[i].Project?.Name ?? "null"}[/]"
                    };
                table.AddRow(sb.ToArray());
            }

            AnsiConsole.Write(table);
        }


        public static void BuildProjectTable(IList<Project> items, string title = "")
        {
            var table = new Table();
            if (!string.IsNullOrWhiteSpace(title)) table.Title = new TableTitle(title.Trim());

            table.AddColumn("Index");
            table.AddColumn("Name");
            table.AddColumn("Project");

            for (int i = 0; i < items.Count; i++)
            {
                string color = "purple";
                List<string> sb = new()
                    {
                        $"[green]{items[i].Id}[/]",
                        $"[{color}]{items[i].Name}[/]",
                        $"[{color}]{items[i].Parent?.Name ?? "null"}[/]"
                    };
                table.AddRow(sb.ToArray());
            }

            AnsiConsole.Write(table);
        }
    }
}
