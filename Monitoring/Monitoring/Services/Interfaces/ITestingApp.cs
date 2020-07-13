using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Monitoring.ViewModels;

namespace Monitoring.Services
{
    public interface ITestingApp
    {
        TestDataJsonList CreateTestData();
    }
}
