using TodoTrack.Contracts;

namespace TodoTrack.Cli
{
    //TODO: should be configured via interface
    public class PomodoroManager
    {
        public PomodoroManager()
        {
            
        }

        private System.Timers.Timer _timer = new();
        private int _workMinutes, _restMinutes, _cycles;
        public void Start(int workMinutes, int restMinutes)
        {
            _timer = new System.Timers.Timer(TimeSpan.FromMinutes(workMinutes));
            _timer.Start();
        }

        public void Stop()
        {
            
        }
    }

}