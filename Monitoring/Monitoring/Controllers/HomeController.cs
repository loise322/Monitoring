using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Monitoring.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Monitoring.ViewModels;
using System.Text.Json;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using NLog;

namespace Monitoring.Controllers
{
    public class HomeController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        TableContext _db;
        public HomeController(TableContext context)
        {
            _db = context;
        }
        public IActionResult Index()
        {
            //VariableAndLogsViewModel VarAndLogsViewModel = new VariableAndLogsViewModel
            //{
            //    Metrics = _db.Metrics,
            //    Logs = _db.Logs,
            //    LastValueOfLogs = (from i in _db.Logs select i.Value).ToList().Last()
            //};
            ViewBag.Title = "Monitoring";
            return View();
        }

        public IActionResult AcceptRequest()
        {
            Random rnd = new Random();
            List<TestDataJson> testDataJson = new List<TestDataJson>
            {
                new TestDataJson { Name = "Name3", IsBoolean = false, WarningThreshold = 30, AlarmThreshold = 120, Priority = PriorityClass.Medium, Kind = "Kind3", Value = rnd.Next(0, 150) },
                new TestDataJson { Name = "Name2", IsBoolean = false, WarningThreshold = 60, AlarmThreshold = 90, Priority = PriorityClass.High, Kind = "Kind2", Value = rnd.Next(0, 120) },
                new TestDataJson { Name = "Name1", IsBoolean = false, WarningThreshold = 5, AlarmThreshold = 12, Priority = PriorityClass.Low, Kind = "Kind1", Value = rnd.Next(0, 30) },
                new TestDataJson { Name = "Name4", IsBoolean = false, WarningThreshold = 20, AlarmThreshold = 30, Priority = PriorityClass.High, Kind = "Kind4", Value = rnd.Next(0, 40) }
            };
            return Json(testDataJson[rnd.Next(0,4)]);
        }

        [HttpPost]
        public IActionResult ProcessData([FromBody]JsonElement JsonData)
        {
            TestDataJson Data = JsonConvert.DeserializeObject<TestDataJson>(JsonData.ToString());
            if (_db.Metrics.Any() && (from i in _db.Metrics select i.Kind).ToList().Contains(Data.Kind))
            {
                LogObject NewLog = new LogObject();
                var AcceptedMetricId = from i in _db.Metrics
                                       where i.Kind == Data.Kind
                                       select i.Id;
                NewLog.MetricId = AcceptedMetricId.First();
                NewLog.Date = DateTime.Now;
                NewLog.Value = Data.Value;
                _db.Logs.Add(NewLog);
                _db.SaveChanges();
                logger.Info($"Log saved! ({Data.Kind})");
                return Ok($"Log saved! ({Data.Kind})");
            }
            if ((!_db.Metrics.Any()) || (_db.Metrics.Any() && !(from i in _db.Metrics select i.Kind).ToList().Contains(Data.Kind)))
            {
                MetricItem NewMetric = new MetricItem
                {
                    Name = "",
                    IsBoolean = Data.IsBoolean,
                    WarningThreshold = Data.WarningThreshold,
                    AlarmThreshold = Data.AlarmThreshold,
                    Priority = Data.Priority,
                    Kind = Data.Kind
                };
                _db.Metrics.Add(NewMetric);
                _db.SaveChanges();
                LogObject NewLog = new LogObject
                {
                    MetricId = (from i in _db.Metrics select i.Id).ToList().Last(),
                    Date = DateTime.Now,
                    Value = Data.Value
                };
                _db.Logs.Add(NewLog);
                _db.SaveChanges();
                logger.Info($"New metric created and that metric's log saved! ({Data.Kind})");
                return Ok($"New metric created and that metric's log saved! ({Data.Kind})");
            }
            return Ok("");
        }
    }
}
