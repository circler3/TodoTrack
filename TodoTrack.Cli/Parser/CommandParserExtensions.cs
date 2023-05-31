using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TodoTrack.Cli.Commands;

namespace TodoTrack.Cli.Parser
{
    public static class CommandParserExtensions
    {
        public static IServiceCollection AddCommandParser(this IServiceCollection services)
        {
            services.AddTransient<CommandParserOption>();
            return services.AddTransient<CommandParser>();
        }

        public static IServiceCollection AddCommandParser(this IServiceCollection services, Action<CommandParserOption> setupAction)
        {
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes()
     .Where(t => !t.IsAbstract && t.GetInterfaces().Any(i => i == typeof(ITodoCommand))))
            {
                services.AddTransient(type);
                //services.AddTransient(type.GetInterfaces().First(i => i == typeof(ICommand)), type);
            }
            services.Configure(setupAction);
            return services.AddTransient<CommandParser>();
        }
    }
}
