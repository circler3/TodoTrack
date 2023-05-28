using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForegroundTimeTracker.Models
{
    public record RegisteredProcess
    {
        public string Name { get; init; } = "";
        public string ProcessName { get; init; } = "";
        public IEnumerable<string>? Keys { get; init; }
        public IList<string> GetWorkFromProcessName(string title)
        {
            return  title.Split(" - ");
        }
    }
}
