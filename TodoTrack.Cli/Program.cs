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
            //services.AddCommandParser(options =>
            //{
            //    options.AddCommand<NewCommand>("new");
            //    options.AddCommand<DelCommand>("del");
            //    options.AddCommand<ListCommand>("list");
            //    options.AddCommand<StartCommand>("start");
            //    options.AddCommand<FinishCommand>("finish");
            //    options.AddCommand<StopCommand>("stop");
            //    options.AddCommand<AddTodoCommand>("add");
            //    options.AddCommand<RemoveCommand>("remove");
            //});
            services.AddAutoMapper(options =>
            {
                options.CreateMap<TodoItem, IndexedTodoItem>();
            }); 
            services.AddTransient<ITodoRepo, TodoSourceRepo>();
            services.AddTransient<ITagRepo, TagSourceRepo>();
            services.AddTransient<IProjectRepo, ProjectSourceRepo>();
            services.AddTransient<IProcessPeriodRepo, ProcessPeriodSourceRepo>();
            services.AddSingleton<TodoHolder>();


            services.AddHostedService<TodoCommandService>();
            services.AddTransient<CommandApp>(w =>
            {
                var app = new CommandApp(new TypeRegistrar(services));
                app.Configure(config =>
                {
                    config.CaseSensitivity(CaseSensitivity.None);
                    config.AddCommand<DelCommand>("del");
                    config.AddBranch<AddSettings>("add", add =>
                    {
                        add.AddCommand<AddTodoCommand>("todo");
                    });
                });
                return app;

            });
        });
    }
}