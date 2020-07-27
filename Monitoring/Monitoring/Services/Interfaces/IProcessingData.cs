using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Models;
using Infrastucture.RabbitMQService;
using Monitoring.ViewModels;

namespace Monitoring.Services
{
    public interface IProcessingData
    {
        void StartProcessingData(PreparedMetrics[] data);
    }
}
