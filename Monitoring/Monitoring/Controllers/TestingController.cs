using System.Linq;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Monitoring.Services;
using Monitoring.ViewModels;

namespace Monitoring.Controllers
{
    public class TestingController : Controller
    {
        private readonly ITestingApp _testingApp;
        private readonly TableContext _db;

        public TestingController(TableContext db, ITestingApp testingApp)
        {
            _testingApp = testingApp;
            _db = db;
        }
        /// <summary>
        /// Источник данных для тестирования программы.
        /// </summary>
        /// <returns>Возвращает JSON данные в представление</returns>
        public IActionResult AcceptRequest()
        {
            return Json(_testingApp.CreateTestData());
        }

        [HttpGet]
        public IActionResult GetTestMetricsForRabbit()
        {
            var model = new MetricsModel
            {
                Metrics = _db.Metrics.ToList()
            };
            return Json(model);
        }
    }
}
