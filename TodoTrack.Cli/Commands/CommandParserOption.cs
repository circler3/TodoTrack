using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace TodoTrack.Cli.Commands
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
            where T : class, ICommand
        {
            CommandDict[v] = typeof(T);
        }

    }
}