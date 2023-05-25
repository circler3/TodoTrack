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
    public class Arrangement : IArrangement
    {
        private readonly ITodoRepo _todoRepo;
        private Queue<WorkFromProcess> _workQueue { get; init; }
        public Arrangement(ITodoRepo todoRepo)
        {
            _workQueue = new();
            this._todoRepo = todoRepo;
        }

        public bool Enqueue(WorkFromProcess process)
        {
            _workQueue.Enqueue(process);
            return true;
        }

        public async Task ArrangeAsync()
        {
            if (_workQueue.Count < 2) return;
            List<WorkFromProcess> workList = new(_workQueue.Count);
            WorkFromProcess lastWorkFromProcess = default;
            while (_workQueue.Count > 1)
            {
                WorkFromProcess process = _workQueue.Dequeue();
                if (process.Duration < TimeSpan.FromMinutes(5)) continue;
                if (lastWorkFromProcess.Title == process.Title) continue;
                lastWorkFromProcess.EndTime = process.StartTime - TimeSpan.FromSeconds(1);
                workList.Add(lastWorkFromProcess);
                lastWorkFromProcess = process; 
            }
            OutputResult(workList);
        }

        private void OutputResult(List<WorkFromProcess> workList)
        {
            Console.WriteLine("*****");
            Console.WriteLine("Writing Results:");
            workList.ForEach(work=> Console.WriteLine(work.Title + $":{work.Duration.TotalMinutes} min."));
            Console.WriteLine("*****");
        }
    }
}
