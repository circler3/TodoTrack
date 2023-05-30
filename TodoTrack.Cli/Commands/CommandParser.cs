using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace TodoTrack.Cli.Commands
{
    public class CommandParser
    {
        private readonly Dictionary<string, Type> _commandDict;
        private readonly IServiceProvider _provider;
        private readonly TodoHolder _todoHolder;

        public CommandParser(IOptions<CommandParserOption> option, IServiceProvider provider, TodoHolder todoHolder)
        {
            _commandDict = option.Value.CommandDict;
            _provider = provider;
            _todoHolder = todoHolder;
        }

        public async ValueTask ExecuteAsync(string cmdStr)
        {
            var cmd = cmdStr.Split(' ', StringSplitOptions.RemoveEmptyEntries)[0].ToLower();
            var jobStr = cmdStr[cmdStr.IndexOf(' ')..].Trim();
            var type =_commandDict[cmd];
            if (type == null) throw new ArgumentNullException(cmd);
            var provider = _provider.CreateScope().ServiceProvider.GetRequiredService<TodoHolder>();
            var cmdImp = _provider.CreateScope().ServiceProvider.GetService(type) as ICommand;
            await cmdImp.ExecuteAsync(jobStr);
        }
    }
}