using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public sealed class StopwatchService
    {
        private static StopwatchService _instance;
        private static readonly object lockObj = new object();
        private Stopwatch _stopwatch;

        private StopwatchService()
        {
            _stopwatch = new Stopwatch();
        }

        public static StopwatchService Instance
        {
            get
            {
                if (_instance is null)
                {
                    lock (lockObj)
                    {
                        if (_instance is null)
                        {
                            _instance = new StopwatchService();
                        }
                    }
                }
                return _instance;
            }
        }

        public void Start()
        {
            _stopwatch.Restart();
        }

        public void Stop()
        {
            _stopwatch.Stop();
        }

        public long ElapsedMilliseconds => _stopwatch.ElapsedMilliseconds;

        public double ElapsedSeconds => _stopwatch.Elapsed.TotalSeconds;

        public void LogElapsed(string message, string unit = "milliseconds")
        {
            string elapsedTime;

            switch (unit.ToLower())
            {
                case "seconds":
                    // F2 indicates number of decimal places to be shown
                    elapsedTime = $"{ElapsedSeconds:F2} seconds";
                    break;
                case "milliseconds":
                default:
                    elapsedTime = $"{ElapsedMilliseconds} ms";
                    break;
            }

            Logger.Instance.Log($"{message} in {elapsedTime}");
        }
    }
}
