using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Spectre.Console.Cli;
using TodoTrack.Cli.Commands;
using TodoTrack.Contracts;

namespace TodoTrack.Cli
{
    public class TodoCommandService : BackgroundService
    {
        private readonly CommandApp _commandApp;

        public TodoCommandService(CommandApp commandApp)
        {
            _commandApp = commandApp;
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
                await _commandApp.RunAsync(cmd.Split(' '));
            }
        }
    }
}