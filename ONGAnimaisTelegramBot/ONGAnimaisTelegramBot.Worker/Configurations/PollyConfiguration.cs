using Polly;
using Polly.Retry;

namespace ONGAnimaisTelegramBot.Worker.Configurations
{
    public static class PollyConfiguration
    {
        public static AsyncRetryPolicy CreateWaitAndRetryPolicy(IEnumerable<TimeSpan> sleepsBeetweenRetries)
        {
            return Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(sleepsBeetweenRetries);
        }
    }
}