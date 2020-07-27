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
            var metricsWithoutValue = _db.Metrics.ToList();
            var metricsWithValue = new List<TestDataJson>();
            foreach (var item in metricsWithoutValue)
            {
                metricsWithValue.Add(new TestDataJson 
                {
                    Id = item.Id,
                    Name = item.Name,
                    IsBoolean = item.IsBoolean,
                    WarningThreshold = item.WarningThreshold,
                    AlarmThreshold = item.AlarmThreshold,
                    Priority = item.Priority,
                    Kind = item.Kind,
                    Value = _db.Logs.Where(i => i.MetricId == item.Id).ToList().Last().Value
                });
            }
            return (new TestDataJsonList { Metrics = metricsWithValue });
        }
    }
}
