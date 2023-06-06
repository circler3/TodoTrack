using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Spectre.Console.Cli;
using TodoTrack.Cli.Helpers;
using TodoTrack.Contracts;
using TodoTrack.TodoDataSource;
using Volo.Abp;

namespace TodoTrack.Cli;

public class Program
{
    public async static Task<int> Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
#if DEBUG
            .MinimumLevel.Debug()
#else
            .MinimumLevel.Information()
#endif
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Async(c => c.File("Logs/logs.txt"))
            .WriteTo.Async(c => c.Console())
            .CreateLogger();

        try
        {
            Log.Information("Starting console host.");

            var builder = Host.CreateDefaultBuilder(args);

            builder.ConfigureServices(services =>
            {
                services.AddHostedService<CliHostedService>();
                services.AddApplicationAsync<CliModule>(options =>
                {
                    options.Services.ReplaceConfiguration(services.GetConfiguration());
                    options.Services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog());

                    options.Services.AddScoped<IRepo<TodoItem>, TodoSourceRepo>();
                    options.Services.AddScoped<IRepo<Tag>, TagSourceRepo>();
                    options.Services.AddScoped<IRepo<Project>, ProjectSourceRepo>();
                    options.Services.AddScoped<IRepo<ProcessPeriod>, ProcessPeriodSourceRepo>();
                    options.Services.AddScoped<TodoHolder>();
                    services.AddCommandApp();
                });
            }).AddAppSettingsSecretsJson().UseAutofac().UseConsoleLifetime();

            var host = builder.Build();
            await host.Services.GetRequiredService<IAbpApplicationWithExternalServiceProvider>().InitializeAsync(host.Services);

            await host.RunAsync();

            return 0;
        }
        catch (Exception ex)
        {
            if (ex is HostAbortedException)
            {
                throw;
            }

            Log.Fatal(ex, "Host terminated unexpectedly!");
            return 1;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
