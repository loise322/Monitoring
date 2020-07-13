using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Monitoring.ViewModels;

namespace Monitoring.Services
{
    public interface IProcessingViewModels
    {
        MetricsModel GetMetricsModel();

        EditMetricModel GetEditMetricModel(int id);

        GraphicModel GetGraphicModel(int id);
    }
}
