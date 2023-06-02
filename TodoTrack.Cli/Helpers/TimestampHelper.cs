using Microsoft.EntityFrameworkCore.Storage;

namespace TodoTrack.Cli
{
    public class TimestampHelper
    {
        internal static long CurrentDateStamp => DateTimeOffset.Now.ToUnixTimeSeconds();
        internal static long? GetDurationFromString(string timeString)
        {
            // 将时间字符串拆分为数字和单位部分
            string numericPart = timeString[..^1];
            string unitPart = timeString[^1..];

            // 解析数字部分为 double 类型
            if (!double.TryParse(numericPart, out double numericValue))
            {
                throw new ArgumentException("Invalid time string: " + timeString);
            }

            // 根据单位计算对应秒数并返回
            return unitPart switch
            {
                "s" => (long?)numericValue,
                "m" => (long?)(numericValue * 60),
                "h" => (long?)(numericValue * 60 * 60),
                "d" => (long?)(numericValue * 24 * 60 * 60),
                _ => throw new ArgumentException("Invalid time unit: " + unitPart),
            };
        }

        public static long GetTodayUnixTimestamp()
        {
            var time = DateTimeOffset.Now;
            return time.ToOffset(-time.TimeOfDay).ToUnixTimeSeconds();
        }
    }
}