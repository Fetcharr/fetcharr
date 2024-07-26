namespace Fetcharr.API.Pipeline
{
    /// <summary>
    ///   Base for timed background services, using periodic intervals.
    /// </summary>
    /// <param name="period">Period of time between invocations.</param>
    public abstract class BasePeriodicService(TimeSpan period, ILogger logger)
        : BackgroundService
    {
        /// <summary>
        ///   Gets the highest about of time between each period, when the service is doing back-off.
        /// </summary>
        private readonly TimeSpan _periodHighLimit = period * 60;

        /// <summary>
        ///   Gets the multiplier for the service period, for each time the service fails or crashes.
        /// </summary>
        private readonly float _periodBackoffMultiplier = 1.2f;

        protected override sealed async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            using PeriodicTimer timer = new(period);

            while(await timer.WaitForNextTickAsync(cancellationToken))
            {
                if(cancellationToken.IsCancellationRequested)
                {
                    break;
                }

                try
                {
                    await this.InvokeAsync(cancellationToken);
                }
                catch(Exception ex)
                {
                    timer.Period *= this._periodBackoffMultiplier;

                    if(timer.Period > this._periodHighLimit)
                    {
                        timer.Period = this._periodHighLimit;
                    }

                    logger.LogError(
                        ex,
                        "Failed to finish periodic task. Increasing back-off to {BackoffTime}.",
                        timer.Period.ToString("c"));
                }

                timer.Period = period;
            }
        }

        /// <summary>
        ///   Callback to invoke every timer interval.
        /// </summary>
        public abstract Task InvokeAsync(CancellationToken cancellationToken);
    }
}