using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ApplicationCore.Models;
using Monitoring.ViewModels;

namespace Monitoring.Services
{
    public interface IDataConverter
    {
        TestDataJsonList DeserializeTestData(JsonElement data);

        MetricItem DeserializeMetric(JsonElement data);
    }
}
