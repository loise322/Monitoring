using System;
using System.Linq;
using System.Text.Json;
using ApplicationCore.Models;
using ApplicationCore.Validators;
using Infrastructure;
using Infrastucture.RabbitMQService;
using Microsoft.AspNetCore.Mvc;
using Monitoring.Services;
using Monitoring.ViewModels;
using NLog;

namespace Monitoring.Controllers
{
    public class MetricController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private readonly TableContext _db;
        private readonly IProcessingData _processingData;
        private readonly IMetricService _metricService;
        private readonly IProcessingViewModels _processingViewModels;
        private readonly IDataConverter _jsonConverters;

        public MetricController(TableContext db, IMetricService metricService, IProcessingData processingData, IProcessingViewModels processingViewModels, IDataConverter jsonConverters)
        {
            _db = db;
            _metricService = metricService;
            _processingData = processingData;
            _jsonConverters = jsonConverters;
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
            _metricService.DeleteMetric(id);
            return Redirect("/View/Metrics");
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

        [HttpGet]
        public IActionResult CreateMetricRequiredNotification()
        {
            return Ok(new MetricsRequiredNotification
            {
                CallingApplication = "Monitoring",
                SourceApplication = "OutsideService",
                TimestampUtc = DateTime.Now,
                SpecificMetricKinds = _db.Metrics.ToList().Select(i => i.Kind).ToArray()
            });
        }

        /// <summary>
        /// Редактирование метрик в базе данных.
        /// </summary>
        /// <param name="data">Принятые JSON данные из представления.</param>
        /// <returns>Возвращает коды состояния или ошибки, которые произошли при валидации</returns>
        [HttpPost]
        public IActionResult EditMetric([FromBody]JsonElement data)
        {
            Validators _validators = new Validators();
            MetricItem dataForEdit = _jsonConverters.DeserializeMetric(data);
            if (dataForEdit == null)
            {
                return BadRequest("Произошла ошибка при десериализации!");
            }
            string validationErrors = _validators.ValidateAll(_validators.SetValidationData(dataForEdit));
            if (validationErrors.Count() == 0)
            {
                _metricService.EditMetric(dataForEdit);
                logger.Info("Changes saved!");
                return Ok();
            }
            return BadRequest(validationErrors);
        }

        /// <summary>
        /// Добавление метрик в базу данных.
        /// </summary>
        /// <param name="data">Принятые JSON данные из представления.</param>
        /// <returns>Возвращает коды состояния или ошибки, которые произошли при валидации</returns>
        [HttpPost]
        public IActionResult AddMetric([FromBody]JsonElement data)
        {
            MetricItem dataForAdd = _jsonConverters.DeserializeMetric(data);
            Validators _validators = new Validators();
            if (dataForAdd == null)
            {
                return BadRequest("Произошла ошибка при десериализации данных!");
            }
            string validationErrors = _validators.ValidateAll(_validators.SetValidationData(dataForAdd));
            if (validationErrors.Count() == 0)
            {
                _metricService.AddMetric(dataForAdd);
                logger.Info($"Metric added! {dataForAdd}");
                return Ok();
            }
            return BadRequest(validationErrors);
        }
    }
}
