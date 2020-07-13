using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Models;
using Infrastructure;
using Monitoring.ViewModels;

namespace Monitoring.Services
{
    public class TestingApp : ITestingApp
    {
        private readonly TableContext _db;

        public TestingApp(TableContext db)
        {
            _db = db;
        }

        public TestDataJsonList CreateTestData()
        {
            Random rnd = new Random();
            if (_db.Metrics.Any())
            {
                var testDataJsonList = new TestDataJsonList();
                var testDataJson = new List<TestDataJson>();
                var metrics = _db.Metrics.ToList();
                foreach (var item in metrics)
                {
                    testDataJson.Add(new TestDataJson { Name = item.Name, IsBoolean = item.IsBoolean, Priority = item.Priority, Kind = item.Kind, WarningThreshold = item.WarningThreshold, AlarmThreshold = item.AlarmThreshold, Value = rnd.Next(0, item.AlarmThreshold + (item.AlarmThreshold - item.WarningThreshold)) });
                };
                testDataJsonList.Metrics = testDataJson;
                return testDataJsonList;
            }
            else
            {
                var testDataJson = new List<TestDataJson>
                {
                    new TestDataJson { Name = "Name3", IsBoolean = false, WarningThreshold = 30, AlarmThreshold = 120, Priority = PriorityKind.Medium, Kind = "Kind3", Value = rnd.Next(0, 150) },
                    new TestDataJson { Name = "Name2", IsBoolean = false, WarningThreshold = 60, AlarmThreshold = 90, Priority = PriorityKind.High, Kind = "Kind2", Value = rnd.Next(0, 120) },
                    new TestDataJson { Name = "Name1", IsBoolean = false, WarningThreshold = 5, AlarmThreshold = 12, Priority = PriorityKind.Low, Kind = "Kind1", Value = rnd.Next(0, 30) },
                    new TestDataJson { Name = "Name4", IsBoolean = false, WarningThreshold = 20, AlarmThreshold = 30, Priority = PriorityKind.High, Kind = "Kind4", Value = rnd.Next(0, 40) }
                };
                var testDataJsonList = new TestDataJsonList
                {
                    Metrics = testDataJson
                };
                return testDataJsonList;
            }
        }
    }
}
