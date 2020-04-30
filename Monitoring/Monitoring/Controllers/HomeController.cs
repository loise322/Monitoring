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
            ViewBag.Title = "Monitoring";
            return View();
        }

        public IActionResult Metrics()
        {
            ViewBag.Title = "Monitoring";
            MetricsModel Model = new MetricsModel();
            Model.Metrics = _db.Metrics.ToList();
            return View(Model);
        }
        public IActionResult Edit(int id)
        {
            ViewBag.Title = "Monitoring";
            if ((from i in _db.Metrics select i.Id).ToList().Contains(id))
            {
                MetricItem Metric = new MetricItem();
                Metric = (from i in _db.Metrics where i.Id == id select i).First();
                EditMetricModel MetricModel = new EditMetricModel
                {
                    Id = Metric.Id,
                    Name = Metric.Name,
                    IsBoolean = Metric.IsBoolean,
                    AlarmThreshold = Metric.AlarmThreshold,
                    WarningThreshold = Metric.WarningThreshold,
                    Priority = Metric.Priority,
                    Kind = Metric.Kind,
                    CheckContain = true
                };
                return View(MetricModel);
            }
            else
            {
                EditMetricModel MetricModel = new EditMetricModel
                {
                    CheckContain = false
                };
                return View(MetricModel);
            }
        }
        [HttpPost]
        public IActionResult EditMetric([FromBody]JsonElement Data)
        {
            MetricItem DataForEdit = JsonConvert.DeserializeObject<MetricItem>(Data.ToString());
            var MetricForEdit = _db.Metrics
                           .Where(i => i.Id == DataForEdit.Id)
                           .FirstOrDefault();
            MetricForEdit.Name = DataForEdit.Name;
            MetricForEdit.IsBoolean = DataForEdit.IsBoolean;
            MetricForEdit.AlarmThreshold = DataForEdit.AlarmThreshold;
            MetricForEdit.WarningThreshold = DataForEdit.WarningThreshold;
            MetricForEdit.Priority = DataForEdit.Priority;
            MetricForEdit.Kind = DataForEdit.Kind;
            _db.SaveChanges();
            logger.Info("Changes saved!");
            return Ok();
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddMetric([FromBody]JsonElement Data)
        {
            MetricItem DataForAdd = JsonConvert.DeserializeObject<MetricItem>(Data.ToString());
            _db.Metrics.Add(DataForAdd);
            _db.SaveChanges();
            return Ok();
        }
        public IActionResult Delete(int id)
        {
            if ((from i in _db.Metrics select i.Id).ToList().Contains(id))
            {
                MetricItem Metric = new MetricItem
                {
                    Id = id
                };
                _db.Metrics.Attach(Metric);
                _db.Metrics.Remove(Metric);
                _db.SaveChanges();
            }
            return Redirect("/Home/Metrics");
        }
        public IActionResult AcceptRequest()
        {
            Random rnd = new Random();
            List<TestDataJson> testDataJson = new List<TestDataJson>
            {
                new TestDataJson { Name = "Name3", IsBoolean = false, WarningThreshold = 30, AlarmThreshold = 120, Priority = PriorityKind.Medium, Kind = "Kind3", Value = rnd.Next(0, 150) },
                new TestDataJson { Name = "Name2", IsBoolean = false, WarningThreshold = 60, AlarmThreshold = 90, Priority = PriorityKind.High, Kind = "Kind2", Value = rnd.Next(0, 120) },
                new TestDataJson { Name = "Name1", IsBoolean = false, WarningThreshold = 5, AlarmThreshold = 12, Priority = PriorityKind.Low, Kind = "Kind1", Value = rnd.Next(0, 30) },
                new TestDataJson { Name = "Name4", IsBoolean = false, WarningThreshold = 20, AlarmThreshold = 30, Priority = PriorityKind.High, Kind = "Kind4", Value = rnd.Next(0, 40) }
            };
            return Json(testDataJson[rnd.Next(0,4)]);
        }

        [HttpPost]
        public IActionResult ProcessData([FromBody]JsonElement JsonData)
        {
            TestDataJson Data = JsonConvert.DeserializeObject<TestDataJson>(JsonData.ToString());
            if (_db.Metrics.Any() && (from i in _db.Metrics select i.Kind).ToList().Contains(Data.Kind))
            {
                LogObject NewLog = new LogObject
                {
                    MetricId = (from i in _db.Metrics where i.Kind == Data.Kind select i.Id).First(),
                    Date = DateTime.Now,
                    Value = Data.Value
                };
                _db.Logs.Add(NewLog);
                _db.SaveChanges();
                logger.Info($"Log saved! ({Data.Kind})");
                return Ok($"Log saved! ({Data.Kind})");
            }
            else
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
        }
    }
}
