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
            const int minutes = 25;
            var timestamp = _db.Logs.Where(i => i.MetricId == id).OrderByDescending(i => i.Id).Take(1).Select(i => i.Date).FirstOrDefault();
            var logsOfMetric = _db.Logs.Where(i => (i.MetricId == id) && (i.Date >= timestamp.AddMinutes(-minutes))).ToList();
            var values = logsOfMetric.Select(i => i.Value);
            var dates = logsOfMetric.Select(i => i.Date.ToString("MM-dd-yyyy HH:mm"));
            var graphicModel = new GraphicModel()
            {
                Name = _db.Metrics.Where(i => i.Id == id).Select(i => i.Name).First()
            };            
            graphicModel.Values = values;
            graphicModel.Labels = dates;
            return graphicModel;
        }
    }
}
