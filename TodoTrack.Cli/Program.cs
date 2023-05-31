using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Spectre.Console.Cli;
using TodoTrack.Cli.Commands;
using TodoTrack.Cli.Parser;
using TodoTrack.Contracts;
using TodoTrack.TodoDataSource;

namespace TodoTrack.Cli
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureServices((hostContext, services) =>
        {
            services.AddCommandParser(options =>
            {
                options.AddCommand<NewCommand>("new");
                options.AddCommand<DelCommand>("del");
                options.AddCommand<ListCommand>("list");
                options.AddCommand<NewCommand>("start");
                options.AddCommand<NewCommand>("finish");
                options.AddCommand<NewCommand>("stop");
                options.AddCommand<AddCommand>("add");
                options.AddCommand<RemoveCommand>("remove");
            });
            services.AddAutoMapper(options =>
            {
                options.CreateMap<TodoItem, IndexedTodoItem>();
            });
            services.AddTransient<ITodoRepo, TodoSourceRepo>();
            services.AddSingleton<TodoHolder>();


            services.AddHostedService<TodoCommandService>();
        });
    }
}