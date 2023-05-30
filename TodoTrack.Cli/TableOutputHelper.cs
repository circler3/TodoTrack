using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoTrack.Cli
{
    internal class TableOutputHelper
    {
        public static void BuildTable(IList<IndexedTodoItem> items, string title = "")
        {
            var table = new Table();
            if(!string.IsNullOrWhiteSpace(title)) table.Title = new TableTitle(title.Trim());

            table.AddColumn(new TableColumn("[red]Focus[/]").Centered());
            table.AddColumn("Index");
            table.AddColumn("Name");
            table.AddColumn("Status");
            table.AddColumn("Project");

            for (int i = 0; i < items.Count; i++)
            {
                List<string> sb = new()
                {
                    items[i].IsFocus ? "[red]>[/]" : "",
                    $"[green]{i}[/]",
                    items[i].Name,
                    items[i].Status.ToString(),
                    items[i].Project?.Name ?? "null"
                };
                table.AddRow(sb.ToArray());
            }

            AnsiConsole.Write(table);
        }
    }
}
