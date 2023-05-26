using ForegroundTimeTracker.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TodoTImeTrack.ForegroundTimeTracker.Models;
using TodoTrack.Contracts;

namespace ForegroundTimeTracker
{
    public class Arrangement : IArrangement
    {
        private readonly IWorkFromProcessRepo _workFromProcessRepo;
        private readonly ITodoRepo _todoRepo;

        private Queue<WorkFromProcess> _workQueue { get; init; }
        public Arrangement(IWorkFromProcessRepo workFromProcessRepo, ITodoRepo todoRepo)
        {
            _workQueue = new();
            _workFromProcessRepo = workFromProcessRepo;
            _todoRepo = todoRepo;
        }

        public bool Enqueue(WorkFromProcess process)
        {
            _workQueue.Enqueue(process);
            return true;
        }

        public async Task ArrangeAsync()
        {
            if (_workQueue.Count < 5) return;
            List<WorkFromProcess> workList = new(_workQueue.Count);
            WorkFromProcess lastWorkFromProcess = _workQueue.Dequeue();
            while (_workQueue.Count > 2)
            {
                var process = _workQueue.Dequeue();
                if (DateTimeOffset.FromUnixTimeSeconds(lastWorkFromProcess.EndTimestamp).Day != DateTimeOffset.FromUnixTimeSeconds(lastWorkFromProcess.StartTimestamp).Day)
                {
                    // additional workitem generate. split it into two individual units.
                    // duplicate last item.
                    var newLastWorkFromProcess = new WorkFromProcess(lastWorkFromProcess)
                    {
                        StartTimestamp = ((DateTimeOffset)DateTimeOffset.FromUnixTimeSeconds(lastWorkFromProcess.EndTimestamp).Date).ToUnixTimeSeconds()
                    };
                    // endtimestamp at next day 00:00:00
                    lastWorkFromProcess.EndTimestamp = ((DateTimeOffset)DateTimeOffset.FromUnixTimeSeconds(lastWorkFromProcess.StartTimestamp).AddDays(1).Date).ToUnixTimeSeconds();
                    workList.Add(lastWorkFromProcess);
                    lastWorkFromProcess = newLastWorkFromProcess;
                }
                if (lastWorkFromProcess.Title == process.Title) continue;
                lastWorkFromProcess.EndTimestamp = process.StartTimestamp;
                //if (process.Duration < TimeSpan.FromMinutes(5)) continue;
                workList.Add(lastWorkFromProcess);
                lastWorkFromProcess = process;
            }
            //For test
            TestOutputResult(workList);
            await _workFromProcessRepo.PostNewEntriesAsync(workList);
            //TODO: Todo management is server side only
            //Match(workList, await _todoRepo.GetTodayTodoItemsAsync(), await _todoRepo.GetCurrentTodoItemAsync());
        }

        private void TestOutputResult(List<WorkFromProcess> workList)
        {
            Console.Clear();
            Console.WriteLine("*****");
            Console.WriteLine("Writing Results:");
            workList.ForEach(work => Console.WriteLine(work.Title + $":{work.Duration.TotalMinutes} min."));
            Console.WriteLine("*****");
        }
    }
}
