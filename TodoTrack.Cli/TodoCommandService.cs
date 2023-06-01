using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Spectre.Console.Cli;
using TodoTrack.Cli.Commands;

namespace TodoTrack.Cli
{
    public class TodoCommandService : BackgroundService
    {
        private readonly IServiceCollection _typeRegistrar;
        private readonly CommandApp _commandApp;

        public TodoCommandService(CommandApp commandApp)
        {
            _commandApp = commandApp;
            //_typeRegistrar = serviceDescriptors;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.Clear();
            //var commandApp = new CommandApp(new TypeRegistrar(_typeRegistrar));
            //commandApp.Configure(config =>
            //{

            //    config.AddBranch<AddSettings>("add", add =>
            //    {
            //        add.AddCommand<AddTodoCommand>("todo");
            //    });
            //});
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