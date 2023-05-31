using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Spectre.Console.Cli;
using TodoTrack.Cli.Parser;

namespace TodoTrack.Cli
{
    public class TodoCommandService : BackgroundService
    {
        private readonly CommandParser _commandParser;

        public TodoCommandService(CommandParser commandParser)
        {
            _commandParser = commandParser;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.Clear();
            while (!stoppingToken.IsCancellationRequested)
            {
                Console.Write("> ");
                var cmd = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(cmd)) continue;

                // 解析命令行参数
                await _commandParser.ExecuteAsync(cmd);
            }
        }
    }
}