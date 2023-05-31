namespace TodoTrack.Contracts
{
    public class WorkPeriod
    {
        public long? StartTimestamp { get; set; }
        public long? EndTimestamp { get; set; }
        public bool Integrity => StartTimestamp.HasValue && EndTimestamp.HasValue && EndTimestamp >= StartTimestamp.Value;
        public bool Started => StartTimestamp.HasValue && !EndTimestamp.HasValue;
    }
}