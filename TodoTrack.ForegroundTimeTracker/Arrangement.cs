using ForegroundTimeTracker.Models;
using System;
using System.Collections.Generic;
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
            TestOutputResult(workList);
            await _workFromProcessRepo.PostNewEntriesAsync(workList);
            //TODO: Todo management is server side only
            //Match(workList, await _todoRepo.GetTodayTodoItemsAsync(), await _todoRepo.GetCurrentTodoItemAsync());
        }

        private void TestOutputResult(List<WorkFromProcess> workList)
        {
            Console.WriteLine("*****");
            Console.WriteLine("Writing Results:");
            workList.ForEach(work => Console.WriteLine(work.Title + $":{work.Duration.TotalMinutes} min."));
            Console.WriteLine("*****");
        }
    }
}
