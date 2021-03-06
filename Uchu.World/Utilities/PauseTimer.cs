using System.Diagnostics;
using System.Timers;

namespace Uchu.World
{
    public class PauseTimer : Timer
    {
        private readonly double _initialInterval;

        private readonly Stopwatch _stopwatch;
        private bool _resumed;

        public PauseTimer(double interval) : base(interval > 0 ? interval : 1)
        {
            interval = interval > 0 ? interval : 1;
            
            _initialInterval = interval;
            
            Elapsed += OnElapsed;
            
            AutoReset = false;
            _stopwatch = new Stopwatch();
        }

        public double RemainingAfterPause { get; private set; }

        public double Time => _stopwatch.ElapsedMilliseconds;

        public new void Start()
        {
            ResetStopwatch();
            base.Start();
        }

        private void OnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            if (_resumed)
            {
                _resumed = false;
                Stop();
                Interval = _initialInterval;
                Start();
            }

            ResetStopwatch();
        }

        private void ResetStopwatch()
        {
            _stopwatch.Reset();
            _stopwatch.Start();
        }

        public void Pause()
        {
            Stop();
            _stopwatch.Stop();
            RemainingAfterPause = Interval - _stopwatch.Elapsed.TotalMilliseconds;
        }

        public void Resume()
        {
            _resumed = true;
            Interval = RemainingAfterPause;
            RemainingAfterPause = 0;
            Start();
        }
    }
}