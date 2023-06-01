using ForegroundTimeTracker.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TodoTrack.Contracts;

namespace ForegroundTimeTracker
{
    public class MonitorWorker : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly IArrangement _aggregationWorker;
        private ProcessPeriod? _lastProcess;

        public MonitorWorker(IConfiguration configuration, IArrangement aggregationWorker)
        {
            _configuration = configuration;
            this._aggregationWorker = aggregationWorker;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var ignoreList = _configuration.GetSection("IgnoreProcessName").Get<string[]>();
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);

                var currentProcess = ProcessHelper.GetForegroundProcess();
                //if (currentProcess == null || string.IsNullOrWhiteSpace(currentProcess.MainWindowTitle)) { continue; }
                if (currentProcess == null) continue;
                if (ignoreList?.Contains(currentProcess.ProcessName) ?? false) 
                    continue;

                if (_lastProcess == null)
                {
                    _lastProcess = new ProcessPeriod(currentProcess);
                    _aggregationWorker.Enqueue(_lastProcess);
                    continue;
                }
                if(_lastProcess.ProcessId != currentProcess.Id || DateTimeOffset.Now.TimeOfDay < TimeSpan.FromSeconds(2))
                {
                    var currentWorkProcess = new ProcessPeriod(currentProcess);
                    _lastProcess.EndTimestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
                    await Console.Out.WriteLineAsync($"Switch to { currentWorkProcess.Title} from {_lastProcess.Title}");
                    _aggregationWorker.Enqueue(currentWorkProcess);
                    _lastProcess = currentWorkProcess;
                }
                if (IdleDetectHelper.GetIdleTime() > TimeSpan.FromMinutes(5)) _lastProcess?.IdlePeriods?.Add(DateTimeOffset.Now.ToUnixTimeSeconds());
            }
        }
    }
}
