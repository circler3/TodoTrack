using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Volo.Abp;
using Spectre.Console.Cli;

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
            await _commandApp.RunAsync(cmd.Split(' '));
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _abpApplication.ShutdownAsync();
    }
}
