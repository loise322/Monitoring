using System.Linq;
using System.Text.Json;
using ApplicationCore.Models;
using ApplicationCore.Validators;
using Microsoft.AspNetCore.Mvc;
using Monitoring.Services;
using Monitoring.ViewModels;
using NLog;

namespace Monitoring.Controllers
{
    public class MetricController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IProcessingData _processingData;
        private readonly IMetricService _metricService;
        private readonly IDataConverter _jsonConverters;

        public MetricController(IMetricService metricService, IProcessingData processingData, IDataConverter jsonConverters)
        {
            _metricService = metricService;
            _processingData = processingData;
            _jsonConverters = jsonConverters;
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

        /// <summary>
        /// Обработка принятых данных. Вследствие чего, добавляются новые метрики,
        /// которых нет в базе данных и добавляются логи в базу данных, если метрика уже существовала в базе даннах.
        /// </summary>
        /// <param name="jsonData">Принятые JSON данные из представления.</param>
        /// <returns>Возвращает код состояния.</returns>
        [HttpPost]
        public IActionResult ProcessData([FromBody]JsonElement jsonData)
        {
            TestDataJsonList data = _jsonConverters.DeserializeTestData(jsonData);
            if (data == null)
            {
                return BadRequest("Произошла ошибка при десериализации!");
            }
            return Ok(_processingData.StartProcessingData(data));
        }
    }
}
