using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Monitoring.Services;
using Monitoring.ViewModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Monitoring.Controllers
{
    [Route("api/[controller]")]
    public class RabbitController : Controller
    {
        private readonly IBusControl _bus;

        public RabbitController(IBusControl bus)
        {
            _bus = bus;
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(MetricsNotification data)
        {
            await _bus.Publish(data);
            return Ok("Success");
        }

    }
}
