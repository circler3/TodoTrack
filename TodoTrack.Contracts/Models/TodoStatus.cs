namespace TodoTrack.Contracts
{
    public enum TodoStatus
    {
        Planned,
        FinishedOnTime,
        FinishedDelayed,
        InProgress,
        Delayed,
        //Scheduled delay
        Postponed,
        Failed,
        Canceled
    }
}