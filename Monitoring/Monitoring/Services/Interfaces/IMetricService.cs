using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Models;
using Infrastructure;
using Monitoring.ViewModels;

namespace Monitoring.Services
{
    public interface IMetricService
    {
        void DeleteMetric(int id);

        void EditMetric(MetricItem metric);

        void AddMetric(MetricItem metric);

    }
}
