using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Models;
using Infrastructure;
using Infrastucture.RabbitMQService;
using Monitoring.ViewModels;
using NLog;

namespace Monitoring.Services
{
    public class ProcessingData : IProcessingData
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private readonly TableContext _db;
        private readonly IMetricService _metricService;

        public ProcessingData(TableContext db, IMetricService metricService)
        {
            _db = db;
            _metricService = metricService;
        }
        public void StartProcessingData(PreparedMetrics[] data)
        {
            var metrics = _db.Metrics.ToList();
            foreach (var item in data)
            {
                if (metrics.Select(i => i.Kind).Contains(item.Kind))
                {
                    AddLog(CreateExistMetricLog(metrics, item));
                    logger.Info($"Log saved! ({item.Kind})");
                }
                else
                {
                    _metricService.AddMetric(CreateNonexistingMetric(item));
                    AddLog(CreateNewMetricLog(item));
                    logger.Info($"New metric created and that metric's log saved! ({item.Kind})");
                }
            };
        }
        
        public void AddLog(LogObject log)
        {
            _db.Logs.Add(log);
            _db.SaveChanges();
        }

        public LogObject CreateExistMetricLog(List<MetricItem> metrics, PreparedMetrics item)
        {
            var log = new LogObject
            {
                MetricId = metrics.Where(i => i.Kind == item.Kind).Select(i => i.Id).First(),
                Date = DateTime.Now,
                Value = item.Value
            };
            return log;
        }

        public LogObject CreateNewMetricLog(PreparedMetrics item)
        {
            var log = new LogObject
            {
                MetricId = _db.Metrics.Select(i => i.Id).ToList().Last(),
                Date = DateTime.Now,
                Value = item.Value
            };
            return log;
        }

        public MetricItem CreateNonexistingMetric(PreparedMetrics item)
        {
            var metric = new MetricItem
            {
                Name = "",
                IsBoolean = false,
                WarningThreshold = 0,
                AlarmThreshold = 0,
                Priority = PriorityKind.Low,
                Kind = item.Kind
            };
            return metric;
        }
    }
}
