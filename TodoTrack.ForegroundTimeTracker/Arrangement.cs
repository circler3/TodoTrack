using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TodoTrack.Contracts;

namespace ForegroundTimeTracker
{
    public class Arrangement : IArrangement
    {
        private readonly IRepo<ProcessPeriod> _processPeriodRepo;

        private Queue<ProcessPeriod> _workQueue { get; init; }
        public Arrangement(IRepo<TodoItem> todoRepo, IRepo<ProcessPeriod> processPeriodRepo)
        {
            _workQueue = new();
            _processPeriodRepo = processPeriodRepo;
        }

        public bool Enqueue(ProcessPeriod process)
        {
            _workQueue.Enqueue(process);
            return true;
        }

        public async Task ArrangeAsync()
        {
            if (_workQueue.Count < 5) return;
            List<ProcessPeriod> workList = new(_workQueue.Count);
            ProcessPeriod lastWorkFromProcess = _workQueue.Dequeue();
            while (_workQueue.Count > 2)
            {
                var process = _workQueue.Dequeue();
                if (DateTimeOffset.FromUnixTimeSeconds(lastWorkFromProcess.EndTimestamp).Day != DateTimeOffset.FromUnixTimeSeconds(lastWorkFromProcess.StartTimestamp).Day)
                {
                    // additional workitem generate. split it into two individual units.
                    // duplicate last item.
                    var newLastWorkFromProcess = new ProcessPeriod(lastWorkFromProcess)
                    {
                        StartTimestamp = ((DateTimeOffset)DateTimeOffset.FromUnixTimeSeconds(lastWorkFromProcess.EndTimestamp).Date).ToUnixTimeSeconds()
                    };
                    // end timestamp at next day 00:00:00
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
            foreach (var work in workList) 
                await _processPeriodRepo.CreateAsync(work);
            //TODO: Todo management is server side only
            //Match(workList, await _todoRepo.GetTodayTodoItemsAsync(), await _todoRepo.GetCurrentTodoItemAsync());
        }

        private void TestOutputResult(List<ProcessPeriod> workList)
        {
            Console.Clear();
            Console.WriteLine("*****");
            Console.WriteLine("Writing Results:");
            Console.WriteLine($"Time consumption (Total {(workList[^1].EndTimestamp - workList[0].StartTimestamp) / 60f:F1} min) " +
                $"since {DateTimeOffset.FromUnixTimeSeconds(workList[0].StartTimestamp).ToLocalTime():t} to {DateTimeOffset.FromUnixTimeSeconds(workList[^1].EndTimestamp).ToLocalTime():t}.");
            Console.WriteLine("*****");
            workList.ForEach(work => Console.WriteLine(work.Name + $" : {work.Duration.TotalMinutes:F2} min."));
            Console.WriteLine("*****");
        }
    }
}
