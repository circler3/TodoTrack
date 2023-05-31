using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
using TodoTrack.Cli.Commands;

namespace TodoTrack.Cli.Parser
{
    public class CommandParserOption
    {
        private readonly Dictionary<string, Type> _commandDict;
        public CommandParserOption()
        {
            _commandDict = new Dictionary<string, Type>();
        }

        public Dictionary<string, Type> CommandDict => _commandDict;

        public void AddCommand<T>(string v)
            where T : class, ITodoCommand
        {
            CommandDict[v] = typeof(T);
        }

    }
}