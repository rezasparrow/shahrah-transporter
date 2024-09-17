using Microsoft.EntityFrameworkCore;
using Shahrah.Framework.Events;
using Shahrah.Framework.Scheduling;
using Shahrah.Transporter.Application.Common.Interfaces;
using Shahrah.Transporter.Application.Vehicles.Services.Interfaces;
using SlimMessageBus;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Transactions;

namespace Shahrah.Transporter.Application.Drivers.EventHandlers;

public class TransactionTestEventHandler : KafkaEventHandler<TestEvent>
{
    private readonly IMessageBus _bus;
    private readonly IJobScheduler _jobScheduler;

    public TransactionTestEventHandler(IMessageBus bus, IJobScheduler jobScheduler)
    {
        _bus = bus;
        _jobScheduler = jobScheduler;
    }

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