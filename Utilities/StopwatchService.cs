using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Utilities
{
    public sealed class StopwatchService
    {

        private readonly Stopwatch _stopwatch;
        private readonly ILogger<StopwatchService> _logger;

        private StopwatchService(ILogger<StopwatchService> logger)
        {
            _stopwatch = new Stopwatch();
            _logger = logger;
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

            _logger.LogInformation($"{message} in {elapsedTime}");
        }
    }
}
