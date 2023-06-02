using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Spectre.Console.Cli;
using TodoTrack.Cli.Commands;
using TodoTrack.Cli.Helpers;
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
            services.AddTransient<IRepo<TodoItem>, TodoSourceRepo>();
            services.AddTransient<IRepo<Tag>, TagSourceRepo>();
            services.AddTransient<IRepo<Project>, ProjectSourceRepo>();
            services.AddTransient<IRepo<ProcessPeriod>, ProcessPeriodSourceRepo>();
            services.AddSingleton<TodoHolder>();


            services.AddHostedService<TodoCommandService>();
            services.AddCommandApp();
        });
    }
}