using Polly;
using Polly.Retry;
using PollyHangFire;

public class ResilientJobConsumer {
    private readonly RetryPolicy _retryPolicy;
    public ResilientJobConsumer() {
        //Build the policy
        _retryPolicy = Policy.Handle<Exception>(
                                 ex => {
                                     Console.WriteLine(ex.Message);
                                     // Conditions to determine whether to continue, for demo we continue anyway
                                     return true;
                                 })
                             .WaitAndRetry(retryCount: 5, sleepDurationProvider: _ => TimeSpan.FromSeconds(1));
    }

    public void ConsumeJob(SimulatedJob job) {
        _retryPolicy.Execute(() => Console.WriteLine(job.DoSomething()));
    }
}
