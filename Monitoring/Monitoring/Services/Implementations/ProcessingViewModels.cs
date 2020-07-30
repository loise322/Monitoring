using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure;
using Monitoring.ViewModels;

namespace Monitoring.Services
{
    public class ProcessingViewModels : IProcessingViewModels
    {
        private readonly TableContext _db;

        public ProcessingViewModels(TableContext db)
        {
            _db = db;
        }

        public MetricsModel GetMetricsModel()
        {
            var model = new MetricsModel
            {
                Metrics = _db.Metrics.ToList()
            };
            return (model);
        }

        public EditMetricModel GetEditMetricModel(int id)
        {
            var metric = _db.Metrics.FirstOrDefault(i => i.Id == id);
            if (metric == null)
            {
                return null;
            }
            var metricModel = new EditMetricModel
            {
                Id = metric.Id,
                Name = metric.Name,
                IsBoolean = metric.IsBoolean,
                AlarmThreshold = metric.AlarmThreshold,
                WarningThreshold = metric.WarningThreshold,
                Priority = metric.Priority,
                Kind = metric.Kind
            };
            return metricModel;
        }

        public GraphicModel GetGraphicModel(int id)
        {
            if (_db.Metrics.Select(i => i.Id).Contains(id))
            {
                if (_db.Logs.Select(i => i.MetricId).Contains(id))
                {
                    return new GraphicModel { MetricId = id };
                }
                return null;
            };
            return null;
        }
    }
}
