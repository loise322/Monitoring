using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ApplicationCore.Models;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Monitoring.ViewModels;
using Newtonsoft.Json;
using TestRabbitMq.Models;

namespace TestRabbitMq.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RabbitController : ControllerBase
    {
        private readonly IBusControl _bus;

        public RabbitController(IBusControl bus)
        {
            _bus = bus;
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(List<TestDataJson> metric)
        {
            Uri uri = new Uri("rabbitmq://localhost/test-data-json");

            var endPoint = await _bus.GetSendEndpoint(uri);
            await endPoint.Send(metric.ToArray());

            return Ok("Success");
        }
    }
}
