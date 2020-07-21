using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure;
using Monitoring.Services;
using Monitoring.ViewModels;

namespace Monitoring.Services
{
    public class GraphicService : IGraphicService
    {
        private readonly TableContext _db;

        public GraphicService(TableContext db)
        {
            _db = db;
        }

        public GraphicModel BuildDataGraphic(int id)
        {
            var logsOfMetric = _db.Logs.Where(i => i.MetricId == id).ToList();
            var allValues = logsOfMetric.Select(i => i.Value);
            var allDate = logsOfMetric.Select(i => i.Date.ToString("MM-dd-yyyy HH:mm"));
            const int maxLength = 50;
            var graphicModel = new GraphicModel()
            {
                Name = _db.Metrics.Where(i => i.Id == id).Select(i => i.Name).First()
            };
            if (allValues.Count() > maxLength)
            {
                graphicModel.Values = allValues.TakeLast(maxLength);
                graphicModel.Labels = allDate.TakeLast(maxLength);
                return graphicModel;
            }
            graphicModel.Values = allValues.TakeLast(allValues.Count());
            graphicModel.Labels = allDate.TakeLast(allValues.Count());
            return graphicModel;
        }
    }
}
