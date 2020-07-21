using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Monitoring.Services;

namespace Monitoring.Controllers
{
    public class TestingController : Controller
    { 
        private readonly ITestingApp _testingApp;

        public TestingController(TableContext context, ITestingApp testingApp)
        {
            _testingApp = testingApp;
        }
        /// <summary>
        /// Источник данных для тестирования программы.
        /// </summary>
        /// <returns>Возвращает JSON данные в представление</returns>
        public IActionResult AcceptRequest()
        {     
            return Json(_testingApp.CreateTestData());      
        }
    }
}
