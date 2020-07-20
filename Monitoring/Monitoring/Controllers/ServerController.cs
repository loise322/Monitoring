using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ApplicationCore.Models;
using ApplicationCore.Validators;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Monitoring.Services;
using Monitoring.ViewModels;
using NLog;

namespace Monitoring.Controllers
{
    public class ServerController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IProcessingData _processingData;
        private readonly IWorkWithData _workWithData;
        private readonly IProcessingGraphic _processingGraphic;
        private readonly IStringValidator _stringValidator;
        private readonly IDataConverter _jsonConverters;

        public ServerController(IWorkWithData workWithData, IProcessingData processingData, IProcessingGraphic processingGraphic, IStringValidator stringValidator, IDataConverter jsonConverters)
        {
            _workWithData = workWithData;
            _processingData = processingData;
            _processingGraphic = processingGraphic;
            _stringValidator = stringValidator;
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
            MetricItem dataForEdit = _jsonConverters.DeserializeMetric(data);
            if (dataForEdit == null)
            {
                return BadRequest("Произошла ошибка при десериализации!");
            }
            string validationErrors = _stringValidator.ValidateStrings(_stringValidator.SetValidationData(dataForEdit));
            if (validationErrors.Count() == 0)
            {
                _workWithData.EditMetric(dataForEdit);
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
            if (dataForAdd == null)
            {
                return BadRequest("Произошла ошибка при десериализации данных!");
            }
            string validationErrors = _stringValidator.ValidateStrings(_stringValidator.SetValidationData(dataForAdd));
            if (validationErrors.Count() == 0)
            {
                _workWithData.AddMetric(dataForAdd);
                logger.Info($"Metric added! {dataForAdd}");
                return Ok();
            }
            return BadRequest(validationErrors);
        }

        /// <summary>
        /// Отправки данных, с помощью которые строится график.
        /// </summary>
        /// <param name="id">Указывает на метрику, график которой нужно построить</param>
        /// <returns>Возвращает JSON массив данных</returns>
        [HttpGet]
        public IActionResult GetDataForGraphic(int id)
        {
            return Json(_processingGraphic.SetDataGraphic(id));
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
