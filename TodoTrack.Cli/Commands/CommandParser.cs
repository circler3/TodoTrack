using Microsoft.Extensions.Options;

namespace TodoTrack.Cli.Commands
{
    public class CommandParser
    {
        private readonly Dictionary<string, string> _commandDict;
        private readonly IServiceProvider _provider;

        public CommandParser(IOptions<CommandParserOption> option, IServiceProvider provider)
        {
            _commandDict = option.Value.CommandDict;
            _provider = provider;
        }

        public async ValueTask ExecuteAsync(string cmdStr)
        {
            var cmd = cmdStr.Split(' ')[0];
            var jobStr = cmdStr[cmdStr.IndexOf(' ')..].Trim();
            var type = Type.GetType(_commandDict[cmd]);
            if (type == null) throw new ArgumentNullException(cmd);
            var cmdImp = _provider.GetService(type) as ICommand;
            await cmdImp.ExecuteAsync(jobStr);
        }
    }
}