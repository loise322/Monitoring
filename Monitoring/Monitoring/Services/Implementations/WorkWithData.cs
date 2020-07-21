using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Models;
using Infrastructure;
using Monitoring.ViewModels;

namespace Monitoring.Services
{
    public class WorkWithData : IWorkWithData
    {
        private readonly TableContext _db;

        public WorkWithData(TableContext db)
        {
            _db = db;
        }

        public void DeleteMetric(int id)
        {
            if (_db.Metrics.Select(i => i.Id).Contains(id))
            {
                var metric = new MetricItem
                {
                    Id = id
                };
                _db.Metrics.Attach(metric);
                _db.Metrics.Remove(metric);
                _db.SaveChanges();
            }
        }

        public void EditMetric(MetricItem metric)
        {
            var metricForEdit = _db.Metrics.FirstOrDefault(i => i.Id == metric.Id);
            metricForEdit.Name = metric.Name;
            metricForEdit.IsBoolean = metric.IsBoolean;
            metricForEdit.AlarmThreshold = metric.AlarmThreshold;
            metricForEdit.WarningThreshold = metric.WarningThreshold;
            metricForEdit.Priority = metric.Priority;
            metricForEdit.Kind = metric.Kind;
            _db.SaveChanges();
        }

        public void AddMetric(MetricItem metric)
        {
            _db.Metrics.Add(metric);
            _db.SaveChanges();
        }
    }
}
