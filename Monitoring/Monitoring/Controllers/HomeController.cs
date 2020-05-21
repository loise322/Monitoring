using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Monitoring.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Monitoring.ViewModels;
using System.Text.Json;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using NLog;
using Monitoring.Validators;
using Monitoring.Classes;

namespace Monitoring.Controllers
{
    public class HomeController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        TableContext _db;

        public HomeController(TableContext context)
        {
            _db = context;
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
            ViewBag.Title = "Monitoring";
            var model = new MetricsModel
            {
                Metrics = _db.Metrics.ToList()
            };
            return View(model);
        }

        /// <summary>
        /// Отображение представления Edit с метрикой, которую редактируем.
        /// </summary>
        /// <param name="id">Указывает на метрику, которую редактируем.</param>
        /// <returns>Возвращает представление с метрикой, которую редактируем.</returns>
        public IActionResult Edit(int id)
        {
            ViewBag.Title = "Monitoring";
            var metric = _db.Metrics.FirstOrDefault(i => i.Id == id);
            if (metric == null)
            {
                return View();
            }
            var metricModel = new EditMetricModel
            {
                Id = metric.Id,
                Name = metric.Name,
                IsBoolean = metric.IsBoolean,
                AlarmThreshold = metric.AlarmThreshold,
                WarningThreshold = metric.WarningThreshold,
                Priority = metric.Priority,
                Kind = metric.Kind
            };
            return View(metricModel);
        }

        /// <summary>
        /// Редактирование метрик в базе данных.
        /// </summary>
        /// <param name="data">Принятые JSON данные из представления.</param>
        /// <returns>Возвращает коды состояния или ошибки, которые произошли при валидации</returns>
        [HttpPost]
        public IActionResult EditMetric([FromBody]JsonElement data)
        {
            var stringValidator = new StringValidator();
            var jsonConverters = new DataConverter();
            MetricItem dataForEdit = jsonConverters.DeserializeMetric(data);
            if (dataForEdit == null)
            {
                return BadRequest("Произошла ошибка при десериализации!");
            }
            var validationData = new List<ValidationData>
            {
                new ValidationData { Name = "Name", Value = dataForEdit.Name, Kind = ValidationKind.MaxLength },
                new ValidationData { Name = "Kind", Value = dataForEdit.Kind, Kind = ValidationKind.MaxLength }
            };
            var metricForEdit = _db.Metrics.FirstOrDefault(i => i.Id == dataForEdit.Id);
            string validationErrors = stringValidator.ValidateStrings(validationData);
            if (validationErrors.Count() == 0)
            {
                metricForEdit.Name = dataForEdit.Name;
                metricForEdit.IsBoolean = dataForEdit.IsBoolean;
                metricForEdit.AlarmThreshold = dataForEdit.AlarmThreshold;
                metricForEdit.WarningThreshold = dataForEdit.WarningThreshold;
                metricForEdit.Priority = dataForEdit.Priority;
                metricForEdit.Kind = dataForEdit.Kind;
                _db.SaveChanges();
                logger.Info("Changes saved!");
                return Ok();
            }
            return BadRequest(validationErrors); 
        }

        /// <summary>
        /// Отображения представления Add.
        /// </summary>
        /// <returns>Возвращает представление Add.</returns>
        public IActionResult Add()
        {
            return View();
        }

        /// <summary>
        /// Добавление метрик в базу данных.
        /// </summary>
        /// <param name="data">Принятые JSON данные из представления.</param>
        /// <returns>Возвращает коды состояния или ошибки, которые произошли при валидации</returns>
        [HttpPost]
        public IActionResult AddMetric([FromBody]JsonElement data)
        {
            var stringValidator = new StringValidator();
            var jsonConverters = new DataConverter();
            MetricItem dataForAdd = jsonConverters.DeserializeMetric(data);
            if (dataForAdd == null)
            {
                return BadRequest("Произошла ошибка при десериализации данных!");
               
            }
            var validationData = new List<ValidationData>
            {
                new ValidationData { Name = "Name", Value = dataForAdd.Name, Kind = ValidationKind.MaxLength },
                new ValidationData { Name = "Kind", Value = dataForAdd.Kind, Kind = ValidationKind.MaxLength }
            };
            string validationErrors = stringValidator.ValidateStrings(validationData);
            if (validationErrors.Count() == 0)
            {
                _db.Metrics.Add(dataForAdd);
                _db.SaveChanges();
                logger.Info($"Metric added! {dataForAdd}");
                return Ok();
            }
            return BadRequest(validationErrors);
        }

        /// <summary>
        /// Удаление метрик из базы данных
        /// </summary>
        /// <param name="id">Указывает на метрику, которую нужно удалить.</param>
        public IActionResult Delete(int id)
        {
            if (_db.Metrics.Select(i => i.Id).Contains(id))
            {
                var metric = new MetricItem
                {
                    Id = id
                };
                _db.Metrics.Attach(metric);
                _db.Metrics.Remove(metric);
                _db.SaveChanges();
            }
            return Redirect("/Home/Metrics");
        }

        /// <summary>
        /// Отображение представления Graphic.
        /// </summary>
        /// <param name="id">Указывает на метрику, график которой нужно построить</param>
        /// <returns>Возвращает представление Graphic</returns>
        public IActionResult Graphic(int id)
        {
            return View(new GraphicModel { MetricId = id });
        }

        /// <summary>
        /// Отправки данных, с помощью которые строится график.
        /// </summary>
        /// <param name="id">Указывает на метрику, график которой нужно построить</param>
        /// <returns>Возвращает JSON массив данных</returns>
        [HttpGet]
        public IActionResult DataForGraphic(int id)
        {
            var allValues = _db.Logs.Where(i => i.MetricId == id).Select(i => i.Value).ToList();
            const int maxLength = 50;
            var graphicModel = new GraphicModel();
            if (allValues.Count() > maxLength)
            {
               
                graphicModel.Values = allValues.TakeLast(maxLength);
                graphicModel.Labels = FillGraphicLabels(maxLength);
                return Json(graphicModel);
            }
            graphicModel.Values = allValues.TakeLast(allValues.Count());
            graphicModel.Labels = FillGraphicLabels(allValues.Count());
            return Json(graphicModel); 
        }

        /// <summary>
        /// Источник данных для тестирования программы.
        /// </summary>
        /// <returns>Возвращает JSON данные в представление</returns>
        public IActionResult AcceptRequest()
        {
            Random rnd = new Random();
            if (_db.Metrics.Any())
            {
                var testDataJsonList = new TestDataJsonList();
                var testDataJson = new List<TestDataJson>();
                var metrics = _db.Metrics.ToList();
                foreach (var item in metrics)
                {
                    testDataJson.Add(new TestDataJson { Name = item.Name, IsBoolean = item.IsBoolean, Priority = item.Priority, Kind = item.Kind, WarningThreshold = item.WarningThreshold, AlarmThreshold = item.AlarmThreshold, Value = rnd.Next(0, item.AlarmThreshold + (item.AlarmThreshold - item.WarningThreshold)) });
                };
                testDataJsonList.Metrics = testDataJson;
                return Json(testDataJsonList); 
            }
            else
            {
                var testDataJson = new List<TestDataJson>
                {
                    new TestDataJson { Name = "Name3", IsBoolean = false, WarningThreshold = 30, AlarmThreshold = 120, Priority = PriorityKind.Medium, Kind = "Kind3", Value = rnd.Next(0, 150) },
                    new TestDataJson { Name = "Name2", IsBoolean = false, WarningThreshold = 60, AlarmThreshold = 90, Priority = PriorityKind.High, Kind = "Kind2", Value = rnd.Next(0, 120) },
                    new TestDataJson { Name = "Name1", IsBoolean = false, WarningThreshold = 5, AlarmThreshold = 12, Priority = PriorityKind.Low, Kind = "Kind1", Value = rnd.Next(0, 30) },
                    new TestDataJson { Name = "Name4", IsBoolean = false, WarningThreshold = 20, AlarmThreshold = 30, Priority = PriorityKind.High, Kind = "Kind4", Value = rnd.Next(0, 40) }
                };
                var testDataJsonList = new TestDataJsonList
                {
                    Metrics = testDataJson
                };
                return Json(testDataJsonList);
            }
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
            var jsonConverters = new DataConverter();
            TestDataJson data = jsonConverters.DeserializeTestData(jsonData);
            if (data == null)
            {
                return BadRequest("Произошла ошибка при десериализации!");
            }
            if (_db.Metrics.Select(i => i.Kind).Contains(data.Kind))
            {
                var newLog = new LogObject
                {
                    MetricId = _db.Metrics.Where(i => i.Kind == data.Kind).Select(i => i.Id).First(),
                    Date = DateTime.Now,
                    Value = data.Value
                };
                _db.Logs.Add(newLog);
                _db.SaveChanges();
                logger.Info($"Log saved! ({data.Kind})");
                return Ok($"Log saved! ({data.Kind})");
            }
            else
            {
                var newMetric = new MetricItem
                {
                    Name = "",
                    IsBoolean = data.IsBoolean,
                    WarningThreshold = data.WarningThreshold,
                    AlarmThreshold = data.AlarmThreshold,
                    Priority = data.Priority,
                    Kind = data.Kind
                };
                _db.Metrics.Add(newMetric);
                _db.SaveChanges();
                var newLog = new LogObject
                {
                    MetricId = _db.Metrics.Select(i => i.Id).ToList().Last(),
                    Date = DateTime.Now,
                    Value = data.Value
                };
                _db.Logs.Add(newLog);
                _db.SaveChanges();
                logger.Info($"New metric created and that metric's log saved! ({data.Kind})");
                return Ok($"New metric created and that metric's log saved! ({data.Kind})");
            }
        }

        /// <summary>
        /// Заполнение ярлыков для графика.
        /// </summary>
        /// <param name="conditionValue">Устанавливает кол-во ярлыков.</param>
        /// <returns>Возвращает список ярлыков</returns>       
        private List<int> FillGraphicLabels(int conditionValue)
        {
            List<int> labels = new List<int>();
            for (int i = 0; i < conditionValue; i++)
            {
                labels.Add(i + 1);
            };
            return labels;
        }
    }
}
