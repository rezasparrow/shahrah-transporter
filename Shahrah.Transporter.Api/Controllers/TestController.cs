using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shahrah.Framework.Events;
using Shahrah.Framework.Scheduling;
using Shahrah.Framework.Services;
using Shahrah.Transporter.Application.Drivers.EventHandlers;
using SlimMessageBus;

namespace Shahrah.Transporter.Api.Controllers
{
    public class TestController(
        INotificationService notificationService,
        IMessageBus bus,
        IJobScheduler jobScheduler) : Controller
    {
        private readonly INotificationService _notificationService = notificationService;
        private readonly IMessageBus _bus = bus;
        private readonly IJobScheduler _jobScheduler = jobScheduler;

        [HttpPost("pushToTc")]
        [AllowAnonymous]
        public async Task<IActionResult> PushTc()
        {
            await _notificationService.AddToTransporterPersonGroup(10, 10);
            await _notificationService.SendToPerson(10, x => x.SearchInWiderArea(2788));

            return Ok();
        }

        [HttpPost("ReqRes")]
        [AllowAnonymous]
        public IActionResult ReqRes()
        {
            return Ok();
        }

        [HttpGet("TestOutbox")]
        [AllowAnonymous]
        public async Task<IActionResult> TestOutbox()
        {
            var ev = new TestEvent(Guid.NewGuid(), "test vlue");
            await _bus.Publish(ev);
            _jobScheduler.ScheduleAt<TransactionTestJob>(b =>
       b.WithIdentifier($"TransactionTestJob_{Guid.NewGuid()}").WithSeconds(5));

            return Ok();
        }
    }
}