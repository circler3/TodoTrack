using ForegroundTimeTracker.Models;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ForegroundTimeTracker
{
    public class AggregationWorker : BackgroundService
    {
        private readonly IArrangement _arrange;

        public AggregationWorker(IArrangement arrange)
        {
            _arrange = arrange;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                
                await Task.Delay(60 * 1000, stoppingToken);
                await _arrange.ArrangeAsync();
            }
        }


    }
}
