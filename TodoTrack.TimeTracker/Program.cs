using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TodoTrack.Contracts;
using TodoTrack.TodoDataSource;

namespace TimeTracker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<IArrangement, Arrangement>();
                    services.AddTransient<IRepo<TodoItem>, TodoSourceRepo>();
                    services.AddTransient<IRepo<Tag>, TagSourceRepo>();
                    services.AddTransient<IRepo<Project>, ProjectSourceRepo>();
                    services.AddTransient<IRepo<ProcessPeriod>, ProcessPeriodSourceRepo>();
                    services.AddHostedService<AggregationWorker>();
                    services.AddHostedService<MonitorWorker>();
                });
    }
}
