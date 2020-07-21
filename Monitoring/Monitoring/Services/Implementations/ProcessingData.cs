using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Models;
using Infrastructure;
using Monitoring.ViewModels;
using NLog;

namespace Monitoring.Services
{
    public class ProcessingData : IProcessingData
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private readonly TableContext _db;
        private readonly IWorkWithData _workWithData;

        public ProcessingData(TableContext db, IWorkWithData workWithData)
        {
            _db = db;
            _workWithData = workWithData;
        }
        public string StartProcessingData(TestDataJsonList data)
        {
            string logs = "";
            var metrics = _db.Metrics.ToList();
            foreach (var item in data.Metrics)
            {
                if (metrics.Select(i => i.Kind).Contains(item.Kind))
                {
                    AddLog(CreateExistMetricLog(metrics, item));
                    logger.Info($"Log saved! ({item.Kind})");
                    logs += $"Log saved! ({item.Kind});\r\n";
                }
                else
                {
                    _workWithData.AddMetric(CreateNonexistingMetric(item));
                    AddLog(CreateNewMetricLog(item));
                    logger.Info($"New metric created and that metric's log saved! ({item.Kind})");
                    logs += $"New metric created and that metric's log saved! ({item.Kind});\r\n";
                }
            };
            return logs.Remove(logs.Length - 2);
        }
        
        public void AddLog(LogObject log)
        {
            _db.Logs.Add(log);
            _db.SaveChanges();
        }

        public LogObject CreateExistMetricLog(List<MetricItem> metrics, TestDataJson item)
        {
            var log = new LogObject
            {
                MetricId = metrics.Where(i => i.Kind == item.Kind).Select(i => i.Id).First(),
                Date = DateTime.Now,
                Value = item.Value
            };
            return log;
        }

        public LogObject CreateNewMetricLog(TestDataJson item)
        {
            var log = new LogObject
            {
                MetricId = _db.Metrics.Select(i => i.Id).ToList().Last(),
                Date = DateTime.Now,
                Value = item.Value
            };
            return log;
        }

        public MetricItem CreateNonexistingMetric(TestDataJson item)
        {
            var metric = new MetricItem
            {
                Name = "",
                IsBoolean = item.IsBoolean,
                WarningThreshold = item.WarningThreshold,
                AlarmThreshold = item.AlarmThreshold,
                Priority = item.Priority,
                Kind = item.Kind
            };
            return metric;
        }
    }
}
