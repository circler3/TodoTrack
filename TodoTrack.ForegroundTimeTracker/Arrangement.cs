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
using TodoTImeTrack.ForegroundTimeTracker.Models;

namespace ForegroundTimeTracker
{
    public class Arrangement : IArrangement
    {
        private readonly IWorkFromProcessRepo _todoRepo;

        private Queue<WorkFromProcess> _workQueue { get; init; }
        public Arrangement(IWorkFromProcessRepo todoRepo)
        {
            _workQueue = new();
            _todoRepo = todoRepo;
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
            while (_workQueue.TryPeek(out var process) && lastWorkFromProcess?.EndTime.Day == process.EndTime.Day)
            {
                process = _workQueue.Dequeue();
                if (process.Duration < TimeSpan.FromMinutes(5)) continue;
                if (lastWorkFromProcess.Title == process.Title) continue;
                lastWorkFromProcess.EndTime = process.StartTime - TimeSpan.FromSeconds(1);
                workList.Add(lastWorkFromProcess);
                lastWorkFromProcess = process; 
            }
            //For test
            OutputResult(workList);
            await  _todoRepo.PostNewEntriesAsync(workList);
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
