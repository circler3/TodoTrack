using Spectre.Console;

namespace TodoTrack.Cli
{
    internal class TableOutputHelper
    {
        public static void BuildTable(IList<IndexedTodoItem> items, string title = "")
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
                        $"[green]{items[i].Index}[/]",
                        $"[{color}]{items[i].Name}[/]",
                        $"[{color}]{items[i].Status}[/]",
                        $"[{color}]{items[i].Project?.Name ?? "null"}[/]"
                    };
                table.AddRow(sb.ToArray());
            }

            AnsiConsole.Write(table);
        }
    }
}
