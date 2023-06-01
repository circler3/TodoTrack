using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;
using TodoTrack.Cli.Commands;
using TodoTrack.Contracts;

namespace TodoTrack.Cli.Helpers
{
    internal static class CommandAppExtensions
    {
        internal static IServiceCollection AddCommandApp(this IServiceCollection services)
        {
            return services.AddTransient(w =>
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
                        add.SetDefaultCommand<NewTodoCommand>();
                        add.AddCommand<NewTodoCommand>("todo");
                        //todo: build
                        add.AddCommand<NewTodoCommand>("pro");
                        //todo: build
                        add.AddCommand<NewTodoCommand>("tag");
                    });
                    config.AddBranch<MethodSettings>("list", list =>
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
        }
    }
}