using System.Diagnostics.CodeAnalysis;
using TodoTrack.Contracts;

namespace TodoTrack.TodoDataSource
{
    /// <summary>
    /// this is temporary use case.
    /// </summary>
    public class WorkPeriodConverter
    {
        public static string ConvertToString(IList<WorkPeriod>? workPeriods)
        {
            if (workPeriods == null) return string.Empty;
            return string.Join(',', workPeriods.Select(w => ConvertToString(w)));
        }
        public static IList<WorkPeriod>? ParseFromString(string str)
        {
            if(string.IsNullOrWhiteSpace(str)) return null;
            return str.Split(',', StringSplitOptions.None)
                .Select(w => Parse(w)).ToList();
        }
        private static string ConvertToString(WorkPeriod period)
        {
            return $"{period.StartTimestamp}/{period.EndTimestamp}";
        }

        private static WorkPeriod Parse(string periodString)
        {
            var str = periodString.Split('/');
            if(str.Length != 2) throw new ArgumentException("Argument incorrect." , nameof(periodString));
            long? start = null;
            long? end = null;
            if(long.TryParse(str[0], out var s)) start = s;
            if(long.TryParse(str[1], out var e)) end = e;
            return new WorkPeriod{StartTimestamp = start, EndTimestamp = end};
        }
    }
}