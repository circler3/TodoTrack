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
                options.AddCommand<NewTodoCommand>("new");
                options.AddCommand<DelTodoCommand>("del");
                options.AddCommand<ListTodoCommand>("list");
                options.AddCommand<NewTodoCommand>("start");
                options.AddCommand<NewTodoCommand>("finish");
                options.AddCommand<NewTodoCommand>("stop");
                options.AddCommand<NewTodoCommand>("add");
                options.AddCommand<NewTodoCommand>("remove");
            });
            services.AddAutoMapper(options =>
            {
                options.CreateMap<TodoItem, IndexedTodoItem>();
            });
            services.AddTransient<ITodoRepo, TodoSourceRepo>();
            services.AddTransient<TodoHolder>();


            services.AddHostedService<TodoCommandService>();
        });
    }
}