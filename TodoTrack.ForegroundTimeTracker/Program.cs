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
                    services.AddSingleton<ITodoRepo, TodoSourceRepo>();
                    services.AddTransient<IWorkFromProcessRepo, TodoSourceRepo>();
                    services.AddHostedService<AggregationWorker>();
                    services.AddHostedService<MonitorWorker>();
                });
    }
}
