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
            return services.AddSingleton(w =>
            {
                var app = new CommandApp(new TypeRegistrar(services));
                app.Configure(config =>
                {
                    config.CaseSensitivity(CaseSensitivity.None);
                    config.AddBranch<IgnoreCategorySettings>("del", del =>
                    {
                        del.SetDefaultCommand<DelCommand<TodoItem>>();
                        del.AddCommand<DelCommand<TodoItem>>("todo");
                        del.AddCommand<DelCommand<Project>>("project").WithAlias("pro");
                        del.AddCommand<DelCommand<Tag>>("tag");
                    });
                    config.AddBranch<IgnoreCategorySettings>("new", add =>
                    {
                        add.SetDefaultCommand<NewTodoCommand>();
                        add.AddCommand<NewTodoCommand>("todo");
                        //todo: build
                        add.AddCommand<NewProjectCommand>("project").WithAlias("pro");
                        //todo: build
                        add.AddCommand<NewTodoCommand>("tag");
                    });
                    config.AddBranch<IgnoreCategorySettings>("list", list =>
                    {
                        list.SetDefaultCommand<ListTodoCommand>();
                        list.AddCommand<ListTodoCommand>("todo");
                        //todo: build
                        list.AddCommand<ListTodoCommand>("project").WithAlias("pro");
                        //todo: build
                        list.AddCommand<ListTodoCommand>("tag");
                    });

                    config.AddCommand<FinishTodoCommand>("finish").WithAlias("f");
                    config.AddCommand<StartTodoCommand>("start").WithAlias("b").WithAlias("begin");
                    config.AddCommand<StopTodoCommand>("stop").WithAlias("s");
                    config.AddCommand<AddTodoCommand>("add").WithAlias("a");
                    config.AddCommand<RemoveTodoCommand>("remove").WithAlias("r");
                });
                return app;

            });
        }
    }
}