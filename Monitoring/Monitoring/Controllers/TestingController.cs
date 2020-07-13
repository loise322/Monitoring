using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Models;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Monitoring.Services;
using Monitoring.ViewModels;

namespace Monitoring.Controllers
{
    public class TestingController : Controller
    { 
        private readonly TableContext _db;
        private readonly ITestingApp _testingApp;

        public TestingController(TableContext context, ITestingApp testingApp)
        {
            _db = context;
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
