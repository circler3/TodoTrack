namespace TodoTrack.Cli.Commands
{
    internal class TimestampHelper
    {
        internal static long CurrentDateStamp => DateTimeOffset.Now.ToUnixTimeSeconds();
        internal static long? GetDurationFromString(string timeString)
        {
            // 将时间字符串拆分为数字和单位部分
            string numericPart = timeString[..^1];
            string unitPart = timeString[^1..];

            // 解析数字部分为 double 类型
            double numericValue;
            if (!double.TryParse(numericPart, out numericValue))
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
    }
}