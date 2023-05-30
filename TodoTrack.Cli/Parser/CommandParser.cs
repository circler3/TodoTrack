using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Spectre.Console;
using TodoTrack.Cli.Commands;

namespace TodoTrack.Cli.Parser
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
            var substrings = cmdStr.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var cmd = substrings[0].ToLower();
            var jobStr = string.Join(" ", substrings.Skip(1));
            if (_commandDict.TryGetValue(cmd, out var type))
            {
                if (type == null) throw new ArgumentNullException(cmd);
                var cmdImp = _provider.CreateScope().ServiceProvider.GetService(type) as ICommand;
                if (cmdImp == null) throw new ArgumentNullException(nameof(cmdImp));
                await cmdImp.ExecuteAsync(jobStr);
            }
            else
                Console.WriteLine("Primary command is invalid.");
        }
    }
}