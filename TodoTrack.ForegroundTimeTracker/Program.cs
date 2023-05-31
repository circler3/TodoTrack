using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TodoTrack.Contracts;
using TodoTrack.TodoDataSource;

namespace ForegroundTimeTracker
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
                    services.AddTransient<ITodoRepo, TodoSourceRepo>();
                    services.AddTransient<ITagRepo, TagSourceRepo>();
                    services.AddTransient<IProjectRepo, ProjectSourceRepo>();
                    services.AddTransient<IProcessPeriodRepo, ProcessPeriodSourceRepo>();
                    services.AddHostedService<AggregationWorker>();
                    services.AddHostedService<MonitorWorker>();
                });
    }
}
