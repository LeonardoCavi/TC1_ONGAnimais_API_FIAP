using Polly.Retry;
using Polly;

namespace ONGAnimaisAPI.API.Configurations
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
