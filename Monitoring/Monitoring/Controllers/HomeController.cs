using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Monitoring.ViewModels;
using System.Text.Json;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using NLog;
using Monitoring.Services;
using Infrastructure;
using ApplicationCore.Models;
using ApplicationCore.Validators;

namespace Monitoring.Controllers
{
    public class HomeController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IWorkWithData _workWithData;
        private readonly IProcessingViewModels _processingViewModels;
        private readonly TableContext _db;

        public HomeController(TableContext context, IWorkWithData workWithData, IProcessingViewModels processingViewModels)
        {
            _db = context;
            _workWithData = workWithData;
            _processingViewModels = processingViewModels;
        }

        /// <summary>
        /// Отображение представления Index. 
        /// </summary>
        /// <returns>Возвращает представление Index.</returns>
        public IActionResult Index()
        {
            ViewBag.Title = "Monitoring";
            return View();
        }

        /// <summary>
        /// Отображение представления Metrics со списком всех метрик, которые есть в базе данных. 
        /// </summary>
        /// <returns>Возвращает представление Metrics со списком всех метрик, которые есть в базе данных.</returns>
        public IActionResult Metrics()
        {
            ViewBag.Title = "Metrics";
            return View(_processingViewModels.GetMetricsModel());
        }

        /// <summary>
        /// Отображение представления Edit с метрикой, которую редактируем.
        /// </summary>
        /// <param name="id">Указывает на метрику, которую редактируем.</param>
        /// <returns>Возвращает представление с метрикой, которую редактируем.</returns>
        public IActionResult Edit(int id)
        {
            ViewBag.Title = "Edit metric";
            return View(_processingViewModels.GetEditMetricModel(id));
        }

        /// <summary>
        /// Отображения представления Add.
        /// </summary>
        /// <returns>Возвращает представление Add.</returns>
        public IActionResult Add()
        {
            ViewBag.Title = "Add metric";
            return View();
        }

        /// <summary>
        /// Удаление метрик из базы данных
        /// </summary>
        /// <param name="id">Указывает на метрику, которую нужно удалить.</param>
        public IActionResult Delete(int id)
        {
            _workWithData.DeleteMetric(id);
            return Redirect("/Home/Metrics");
        }

        /// <summary>
        /// Отображение представления Graphic.
        /// </summary>
        /// <param name="id">Указывает на метрику, график которой нужно построить</param>
        /// <returns>Возвращает представление Graphic</returns>
        public IActionResult Graphic(int id)
        {
            ViewBag.Title = $"Graphic of {id} metric";
            return View(_processingViewModels.GetGraphicModel(id));
        }
    }
}
