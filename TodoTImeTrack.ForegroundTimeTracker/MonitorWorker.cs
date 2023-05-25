using ForegroundTimeTracker.Models;
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


namespace ForegroundTimeTracker
{
    public class MonitorWorker : BackgroundService
    {
        private readonly ILogger<MonitorWorker> _logger;
        private readonly IConfiguration _configuration;
        private readonly IArrangement _aggregationWorker;
        private WorkFromProcess _lastProcess;

        public MonitorWorker(ILogger<MonitorWorker> logger, IConfiguration configuration, IArrangement aggregationWorker)
        {
            _logger = logger;
            _configuration = configuration;
            this._aggregationWorker = aggregationWorker;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var ignoreList = _configuration.GetSection("IgnoreProcessName").Get<string[]>();
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(500, stoppingToken);

                var currentProcess = ProcessHelper.GetForegroundProcess();
                if (currentProcess == null || string.IsNullOrWhiteSpace(currentProcess.MainWindowTitle)) { continue; }
                if (ignoreList.Contains(currentProcess.ProcessName)) 
                    continue;

                if (_lastProcess == null)
                {
                    _lastProcess = new WorkFromProcess(currentProcess);
                    _aggregationWorker.Enqueue(_lastProcess);
                    continue;
                }
                if(_lastProcess.ProcessId != currentProcess.Id)
                {
                    var currentWorkProcess = new WorkFromProcess(currentProcess);
                    _lastProcess.EndTime = DateTimeOffset.Now;
                    await Console.Out.WriteLineAsync($"Switch to { currentWorkProcess.Title} from {_lastProcess.Title}");
                    _aggregationWorker.Enqueue(currentWorkProcess);
                    _lastProcess = currentWorkProcess;
                }
                if (IdleDetectHelper.GetIdleTime() > TimeSpan.FromMinutes(5)) _lastProcess.IdlePeriods.Add(DateTimeOffset.Now);
            }
        }
    }
}
