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
            services.AddTransient<IRepo<TodoItem>, TodoSourceRepo>();
            services.AddTransient<IRepo<Tag>, TagSourceRepo>();
            services.AddTransient<IRepo<Project>, ProjectSourceRepo>();
            services.AddTransient<IRepo<ProcessPeriod>, ProcessPeriodSourceRepo>();
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
                        del.SetDefaultCommand<DelCommand<TodoItem>>();
                        del.AddCommand<DelCommand<TodoItem>>("todo");
                        del.AddCommand<DelCommand<Project>>("pro");
                        del.AddCommand<DelCommand<Tag>>("tag");
                    });
                    config.AddBranch<MethodSettings>("new", add =>
                    {
                        add.SetDefaultCommand<NewCommand>();
                        add.AddCommand<NewCommand>("todo");
                        //todo: build
                        add.AddCommand<NewCommand>("pro");
                        //todo: build
                        add.AddCommand<NewCommand>("tag");
                    });
                    config.AddBranch<MethodSettings>("list", list=>
                    {
                        list.SetDefaultCommand<ListCommand>();
                        list.AddCommand<ListCommand>("todo");
                        //todo: build
                        list.AddCommand<ListCommand>("pro");
                        //todo: build
                        list.AddCommand<ListCommand>("tag");
                    });

                    config.AddCommand<FinishTodoCommand>("finish");
                    config.AddCommand<StartTodoCommand>("start");
                    config.AddCommand<StopTodoCommand>("stop");
                    config.AddCommand<AddTodoCommand>("add");
                    config.AddCommand<RemoveTodoCommand>("remove");
                });
                return app;

            });
        });
    }
}