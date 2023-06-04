using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Volo.Abp;
using Spectre.Console.Cli;
using System.Text;

namespace TodoTrack.Cli;

public class CliHostedService : IHostedService
{
    private readonly IAbpApplicationWithExternalServiceProvider _abpApplication;
    private readonly CommandApp _commandApp;

    public CliHostedService(CommandApp commandApp, IAbpApplicationWithExternalServiceProvider abpApplication)
    {
        _commandApp = commandApp;
        _abpApplication = abpApplication;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        Console.Clear();
        while (!cancellationToken.IsCancellationRequested)
        {
            Console.Write("> ");
            var cmd = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(cmd)) continue;

            // 解析命令行参数
            await _commandApp.RunAsync(Parse(cmd));
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _abpApplication.ShutdownAsync();
    }

    public static string[] Parse(string commandLine)
    {
        // Split the command-line string into an array of strings
        List<string> args = new List<string>();
        bool insideQuotes = false;
        StringBuilder currentArg = new StringBuilder();

        for (int i = 0; i < commandLine.Length; i++)
        {
            char c = commandLine[i];

            if (c == '"')
            {
                insideQuotes = !insideQuotes;
                continue;
            }

            if (c == ' ' && !insideQuotes)
            {
                if (currentArg.Length > 0)
                {
                    args.Add(currentArg.ToString());
                    currentArg.Clear();
                }
                continue;
            }

            currentArg.Append(c);
        }

        if (currentArg.Length > 0)
        {
            args.Add(currentArg.ToString());
        }
        return args.ToArray();
    }

}
