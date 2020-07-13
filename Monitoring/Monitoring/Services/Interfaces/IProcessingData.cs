using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Models;
using Monitoring.ViewModels;

namespace Monitoring.Services
{
    public interface IProcessingData
    {
        string StartProcessingData(TestDataJsonList data);
    }
}
