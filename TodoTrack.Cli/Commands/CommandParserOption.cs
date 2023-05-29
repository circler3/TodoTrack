namespace TodoTrack.Cli.Commands
{
    public class CommandParserOption
    {
        private readonly Dictionary<string, string> _commandDict;
        public CommandParserOption()
        {
            _commandDict = new Dictionary<string, string>();
        }

        public Dictionary<string, string> CommandDict => _commandDict;

        public void AddCommand<T>(string v)
            where T : ICommand
        {
            CommandDict[v] = nameof(T);
        }
    }
}