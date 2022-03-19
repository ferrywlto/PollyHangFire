using Hangfire;
using PollyHangFire;

Console.WriteLine("Start");

GlobalConfiguration.Configuration.UseInMemoryStorage();

RecurringJob.AddOrUpdate<ResilientJobConsumer>("poll", (jc) => jc.ConsumeJob(new SimulatedJob()), Cron.Minutely);

using var bjServer = new BackgroundJobServer();

Console.ReadLine();
