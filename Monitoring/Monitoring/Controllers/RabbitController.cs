using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure;
using Infrastucture.RabbitMQService;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Monitoring.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RabbitController : Controller
    {
        private readonly IBusControl _bus;

        public RabbitController(IBusControl bus)
        {
            _bus = bus;
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(MetricsRequiredNotification data)
        {
            Uri uri = new Uri("rabbitmq://localhost/test-data-required");

            var endPoint = await _bus.GetSendEndpoint(uri);
            await endPoint.Send(data);
            return Ok("Success");
        }
    }
}
