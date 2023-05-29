using Microsoft.Extensions.Hosting;
using Spectre.Console.Cli;

namespace TodoTrack.Cli
{
    public class TodoCommandService : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (stoppingToken.IsCancellationRequested)
            {
                Console.Write("> ");
                var cmd = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(cmd)) continue;

                // 解析命令行参数
                List<string> arguments = new List<string>(cmd.Split(' ', StringSplitOptions.RemoveEmptyEntries));
                string action = arguments[0];
                string remains = cmd[cmd.IndexOf(action)..].Trim();

                switch (action.ToLower())
                {
                    //case "add":
                    //    AddInstance(arguments);
                    //    break;

                    //case "remove":
                    //    RemoveInstance(arguments);
                    //    break;

                    default:
                        Console.WriteLine($"未知命令: {action}");
                        break;
                }
            }
        }
    }
}