using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Spectre.Console.Cli;
using TodoTrack.Cli.Commands;
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
                options.AddCommand<AddTodoCommand>("add");
                options.AddCommand<AddTodoCommand>("new");
                //options.AddCommand<AddTodoCommand>("del");
                //options.AddCommand<AddTodoCommand>("start");
                //options.AddCommand<AddTodoCommand>("finish");
                //options.AddCommand<AddTodoCommand>("stop");
                //options.AddCommand<AddTodoCommand>("remove");
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