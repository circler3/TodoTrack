using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TodoTImeTrack.ForegroundTimeTracker.Models;
using TodoTrack.Contracts;
using TodoTrack.ForegroundTimeTracker;
using TodoTrack.ForegroundTimeTracker.Models;

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
                    services.AddDbContext<SQLiteDbContext>(options=>
                    {
                        options.UseSqlite("Data Source=test.db");
                    });
                    services.AddSingleton<IArrangement, Arrangement>();
                    services.AddSingleton<ITodoRepo, TodoRepo>();
                    services.AddTransient<IWorkFromProcessRepo, WorkFromProcessRepo>();
                    services.AddHostedService<AggregationWorker>();
                    services.AddHostedService<MonitorWorker>();
                });
    }
}
