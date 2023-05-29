using Spectre.Console;
using System.Diagnostics.CodeAnalysis;
using TodoTrack.Contracts;

namespace TodoTrack.Cli.Commands
{
    public interface ICommand
    {
        public Task<int> ExecuteAsync([NotNull] string command);
    }
}