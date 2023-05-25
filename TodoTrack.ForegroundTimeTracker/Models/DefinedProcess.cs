using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForegroundTimeTracker.Models
{
    public record DefinedProcess
    {
        public string Name { get; init; } = "";
        public string MatchRegex { get; init; } = @"(.+)";
        public string OutputRegex { get; init; } = @"(.+)";
        public int Order { get; init; } = 0;
        public IEnumerable<string> Tags { get; init; }
        public IList<string> GetWorkFromProcessName(string title)
        {
            return  title.Split(" - ");
        }
    }
}
