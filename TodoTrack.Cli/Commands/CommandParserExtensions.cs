using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoTrack.Cli.Commands
{
    public static class CommandParserExtensions
    {
        public static IServiceCollection AddCommandParser(this IServiceCollection services)
        {
            services.AddTransient<CommandParserOption>();
            return services.AddSingleton<CommandParser>();
        }

        public static IServiceCollection AddCommandParser(this IServiceCollection services, Action<CommandParserOption> setupAction)
        {
            services.Configure(setupAction);
            return services.AddTransient<CommandParser>();
        }
    }
}
