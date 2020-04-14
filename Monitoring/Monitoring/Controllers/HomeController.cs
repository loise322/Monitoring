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
            //VariableAndLogsViewModel VarAndLogsViewModel = new VariableAndLogsViewModel();
            //VarAndLogsViewModel.Metrics = _db.Metrics;
            //VarAndLogsViewModel.Logs = _db.Logs;
            //VarAndLogsViewModel.LastValueOfLogs = (from i in _db.Logs select i.Value).ToList().Last();
            ViewBag.Title = "Monitoring";
            return View();
        }

        public IActionResult AcceptRequest()
        {
            Random rnd = new Random();
            List<TestDataJson> testDataJson = new List<TestDataJson>
            {
                new TestDataJson { Name = "Name3", IsBoolean = false, WarningThreshold = 30, AlarmThreshold = 120, Priority = 0, Kind = "Kind0", Value = rnd.Next(0, 150) },
                new TestDataJson { Name = "Name2", IsBoolean = false, WarningThreshold = 60, AlarmThreshold = 90, Priority = 1, Kind = "Kind0", Value = rnd.Next(0, 120) },
                new TestDataJson { Name = "Name1", IsBoolean = false, WarningThreshold = 5, AlarmThreshold = 12, Priority = 1, Kind = "Kind0", Value = rnd.Next(0, 30) }
            };
            return Json(testDataJson[rnd.Next(0, 3)]);
        }

        [HttpPost]
        public IActionResult ProcessData([FromBody]JsonElement JsonData)
        {
            TestDataJson Data = JsonConvert.DeserializeObject<TestDataJson>(JsonData.ToString());
            LogObject NewLog = new LogObject();
            List<string> ListOfKinds = (from i in _db.Metrics select i.Kind).ToList();
            try
            {
                var AcceptedMetricId = from i in _db.Metrics
                                       where i.Kind == Data.Kind
                                       select i.Id;
                NewLog.MetricId = AcceptedMetricId.First();
                foreach (var Kind in ListOfKinds.Where(i => i == Data.Kind))
                {
                    if (Kind == Data.Kind)
                    {
                        NewLog.Date = DateTime.Now;
                        NewLog.Value = Data.Value;
                        _db.Logs.Add(NewLog);
                        _db.SaveChanges();
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Info(ex.Message);
                return Ok(ex.Message);
            }
            return Ok("Logs saved!");
        }
    }
}
