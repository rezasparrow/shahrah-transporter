using Shahrah.Framework.Events;
using Shahrah.Framework.Scheduling;
using SlimMessageBus;
using System.Diagnostics;

namespace Shahrah.Transporter.Application.Drivers.EventHandlers;

public class TransactionTestEventHandler(IMessageBus bus, IJobScheduler jobScheduler) : KafkaEventHandler<TestEvent>
{
    private readonly IMessageBus _bus = bus;
    private readonly IJobScheduler _jobScheduler = jobScheduler;

    public override async Task Handle(TestEvent message)
    {
        await _bus.Publish(new TestEvent2(Guid.NewGuid(), "Test Event 2"));

             _jobScheduler.ScheduleAt<TransactionTestJob>(b =>
                b.WithIdentifier($"TransactionTestJob_{Guid.NewGuid()}").WithSeconds(5));

    }
}

public class TransactionTestJob : DelayedJob
{
    public override Task RunAsync(Dictionary<string, string> data)
    {
       return  Task.Run(()=>
        {
            Debug.Write("TransactionTestJob executed succesfully.");
        });
    }
}