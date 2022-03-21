# PollyHangFire
To demonstrate how to combine Hangfire and Polly to create a minimal automation system.

## Your automation buddies in C#

![Polly](https://miro.medium.com/max/508/0*pAzO-cChcxzLG3JL.png)

Hello everyone! Welcome to my `#Everything in CSharp` series. This time we write fewer code than before because we want our computer work for us. One of the programmers' task is to automate things and to reduce human workload. Let's say you want to monitor some website data, but implementing a useful scheduler is not an easy task. Unix/Linux power users may probably know cron jobs, today we will learn how to do it in C#.
Let's start create a C# project and a simulated job.

https://github.com/ferrywlto/PollyHangFire/blob/6305256b5d3446511d7992ec3555fe534b74a4c4/SimulatedJob.cs#L1-L14

```sh
dotnet new console
touch SimulatedJob.cs
```
The job simply return an integer on every 5th call. Otherwise it will throw an exception. This simulate some remote lengthy process under poor network connection.
Handling retry and recovery of calling such service could be a tedious work. Luckily we have our first buddy today: [Polly](http://www.thepollyproject.org/)

From the project’s [GitHub repository site](https://github.com/App-vNext/Polly):

> Polly is a .NET resilience and transient-fault-handling library that allows developers to express policies such as Retry, Circuit Breaker, Timeout, Bulkhead Isolation, and Fallback in a fluent and thread-safe manner.

Polly is open-source, under active development and popular (more than 10K stars) among .NET developers. Let’s add Polly into our project.

```sh
dotnet add package polly
```

Now we can write the code to consume our not that reliable job:

https://github.com/ferrywlto/PollyHangFire/blob/6305256b5d3446511d7992ec3555fe534b74a4c4/ResilientJobConsumer.cs#L1-L21

First we need to create a retry policy to handle when the service we call throws exceptions. In our example we simply print the exception message and then continue. We also set how many times we will retry and how long to wait before next retry in the chaining method `WaitAndRetry()` .

Now we make our call become resilient and able to recover from transient errors. To build a simple automation system, we need not only a scheduler, more ideally it should also able to manage multiple scheduled tasks. If you want to implement these by your own it will take a long period of time I guess. The good news is the second buddy I introduce today have you all covered!
[Hangfire](https://www.hangfire.io/)

Hangfire is free, open-source, lightweight library not only can run schedule jobs for you, it also handles queuing. It can even resume your queued jobs in persistent storage. Form their [GitHub repository page](https://github.com/HangfireIO/Hangfire):

> An easy way to perform background job processing in your .NET and .NET Core applications. No Windows Service or separate process required.

To add Hangfire in our project:

```sh
dotnet add package HangFire
```

We also use in memory storage instead of real database for demo purpose:
```sh
dotnet add package HangFire.InMemory
```
To demonstrate how simple you can integrate with Hangfire, open `Program.cs` and edit like below:

https://github.com/ferrywlto/PollyHangFire/blob/6305256b5d3446511d7992ec3555fe534b74a4c4/Program.cs#L1-L12

Note that at line 5 we config Hangfire with in memory storage instead of persistent datastore like SQL Server. Then we setup a recurring job to execute once per minute at line 7. If you are familiar will cron job syntax, you can pass `"* * * * *"` instead of `Cron.Minutely` .
Then we need to instantiate a `BackgroundJobServer` object. This object is required to kick start all our scheduled jobs.
Finally since this is a console program, we need a way to keep the program up and running in order to wait and see the job executions. The line `Console.ReadLine()` at the end is for this purpose. In project like ASP.NET it is not needed.

Let's see our minimal automation program in action:
```sh
dotnet run
```

Wait 2 minutes and you will see the job is running:

![result in action](https://miro.medium.com/max/442/1*qUrCGR6J3mNamuUzdTCqsw.png)

On every minute Hangfire execute the job we wrapped in Polly, it will fail four times and then return multiple of 5 on the 5th attempt.


Today we learned how to combine Hangfire and Polly to create a minimal automated system. By mixing various job type, scheduling options, queuing and recovery policy you can build a very complex system to fit your automation needs. The sky's the limit.


That's it for today and hope you enjoy it. Stay tuned for my next #Everything in CSharp series! See you next time. 😃
