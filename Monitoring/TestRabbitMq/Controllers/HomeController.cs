using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Models;
using Microsoft.AspNetCore.Mvc;
using Monitoring.ViewModels;

namespace TestRabbitMq.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetTestData(int count)
        {
            Random rnd = new Random();
            var testDataJson = new List<TestDataJson>();
            for (int i = 0; i < count; i++)
            {
                testDataJson.Add(new TestDataJson { Name = "Name" + i, IsBoolean = false, WarningThreshold = 8, AlarmThreshold = 20, Priority = PriorityKind.Medium, Kind = "Kind" + i, Value = rnd.Next(0,30)});
            };
            return Json(testDataJson);
        }
    }
}
