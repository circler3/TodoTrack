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
                    config.AddBranch<MethodSettings>("del", del =>
                    {
                        del.AddCommand<AddTodoCommand>("todo");
                        del.AddCommand<AddTodoCommand>("proj");
                        del.AddCommand<AddTodoCommand>("tag");
                    });
                    config.AddBranch<MethodSettings>("add", add =>
                    {
                        add.AddCommand<AddTodoCommand>("todo");
                        add.AddCommand<AddTodoCommand>("proj");
                        add.AddCommand<AddTodoCommand>("tag");
                    });
                });
                return app;

            });
        });
    }
}