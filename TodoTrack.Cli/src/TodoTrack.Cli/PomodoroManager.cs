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
        private bool _work;
        public void Start(int workMinutes, int restMinutes, int cycles)
        {
            _workMinutes = workMinutes;
            _restMinutes = restMinutes;
            _cycles = cycles;
            _timer = new System.Timers.Timer(TimeSpan.FromMinutes(workMinutes));
            _timer.Elapsed += TimerElaped;
            _timer.AutoReset = false;
            _work = true;
            _timer.Start();
        }

        private void TimerElaped(object? sender, System.Timers.ElapsedEventArgs eventArgs)
        {
            Console.WriteLine("OK");
            if (_work)
            {
                if (_cycles > 0)
                {
                    _cycles--;
                    _timer.Interval = _restMinutes * 60 * 1000;
                    _timer.Start();
                }
                else
                {
                    Console.WriteLine("FinishFocus");
                }
            }
            else
            {
                _timer.Interval = _workMinutes * 60 * 1000;
                _timer.Start();
            }

        }

        public void Stop()
        {
            _timer.Stop();
        }
    }

}