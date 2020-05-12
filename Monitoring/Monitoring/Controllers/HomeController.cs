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
            MetricsModel Model = new MetricsModel
            {
                Metrics = _db.Metrics.ToList()
            };
            return View(Model);
        }

        /// <summary>
        /// Отображение представления Edit с метрикой, которую редактируем.
        /// </summary>
        /// <param name="id">Указывает на метрику, которую редактируем.</param>
        /// <returns>Возвращает представление с метрикой, которую редактируем.</returns>
        public IActionResult Edit(int id)
        {
            ViewBag.Title = "Monitoring";
            MetricItem Metric = _db.Metrics.FirstOrDefault(i => i.Id == id);
            if (Metric != null)
            {
                EditMetricModel MetricModel = new EditMetricModel
                {
                    Id = Metric.Id,
                    Name = Metric.Name,
                    IsBoolean = Metric.IsBoolean,
                    AlarmThreshold = Metric.AlarmThreshold,
                    WarningThreshold = Metric.WarningThreshold,
                    Priority = Metric.Priority,
                    Kind = Metric.Kind
                };
                return View(MetricModel);
            }
            return View();
        }

        /// <summary>
        /// Редактирование метрик в базе данных.
        /// </summary>
        /// <param name="Data">Принятые JSON данные из представления.</param>
        /// <returns>Возвращает коды состояния или ошибки, которые произошли при валидации</returns>
        [HttpPost]
        public IActionResult EditMetric([FromBody]JsonElement Data)
        {
            StringValidator stringValidator = new StringValidator();
            JsonConverters jsonConverters = new JsonConverters();
            var DataForEdit = jsonConverters.DeserializeMetric(Data);
            if (DataForEdit != null)
            {
                var MetricForEdit = _db.Metrics.FirstOrDefault(i => i.Id == DataForEdit.Id);
                var ValidationErrors = stringValidator.ValidateStrings(DataForEdit.Name, DataForEdit.Kind);
                if (ValidationErrors.Count() == 0)
                {
                    MetricForEdit.Name = DataForEdit.Name;
                    MetricForEdit.IsBoolean = DataForEdit.IsBoolean;
                    MetricForEdit.AlarmThreshold = DataForEdit.AlarmThreshold;
                    MetricForEdit.WarningThreshold = DataForEdit.WarningThreshold;
                    MetricForEdit.Priority = DataForEdit.Priority;
                    MetricForEdit.Kind = DataForEdit.Kind;
                    _db.SaveChanges();
                    logger.Info("Changes saved!");
                    return Ok();
                }
                else
                {
                    var ValidationErrorsToJson = jsonConverters.SerializeErrors(ValidationErrors.ToList());
                    if (ValidationErrorsToJson != null)
                    {
                        return BadRequest(ValidationErrorsToJson);
                    }
                    return StatusCode(500, "Произошла ошибка при сериализации ошибок валидатора!");
                }
            }
            return StatusCode(500, "Произошла ошибка при десериализации");
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
        /// <param name="Data">Принятые JSON данные из представления.</param>
        /// <returns>Возвращает коды состояния или ошибки, которые произошли при валидации</returns>
        [HttpPost]
        public IActionResult AddMetric([FromBody]JsonElement Data)
        {
            StringValidator stringValidator = new StringValidator();
            JsonConverters jsonConverters = new JsonConverters();
            var DataForAdd = jsonConverters.DeserializeMetric(Data);
            if (DataForAdd != null)
            {
                var ValidationErrors = stringValidator.ValidateStrings(DataForAdd.Name, DataForAdd.Kind);
                if (ValidationErrors.Count() == 0)
                {
                    _db.Metrics.Add(DataForAdd);
                    _db.SaveChanges();
                    logger.Info($"Metric added! {DataForAdd}");
                    return Ok();
                }
                else
                {
                    var ValidationErrorsToJson = jsonConverters.SerializeErrors(ValidationErrors.ToList());
                    if (ValidationErrorsToJson != null)
                    {
                        return BadRequest(ValidationErrorsToJson);
                    }
                    return StatusCode(500, "Произошла ошибка при сериализации ошибок валидатора!");
                }
            }
            return StatusCode(500, "Произошла ошибка при десериализации данных!");
        }

        /// <summary>
        /// Удаление метрик из базы данных
        /// </summary>
        /// <param name="id">Указывает на метрику, которую нужно удалить.</param>
        public IActionResult Delete(int id)
        {
            if (_db.Metrics.Select(i => i.Id).Contains(id))
            {
                MetricItem Metric = new MetricItem
                {
                    Id = id
                };
                _db.Metrics.Attach(Metric);
                _db.Metrics.Remove(Metric);
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
            var AllValues = _db.Logs.Where(i => i.MetricId == id).Select(i => i.Value).ToList();
            const int MaxLength = 50;
            GraphicModel graphicModel = new GraphicModel();
            if (AllValues.Count() > MaxLength)
            {
               
                graphicModel.Values = AllValues.TakeLast(MaxLength);
                graphicModel.Labels = FillGraphicLabels(MaxLength);
                return Json(graphicModel);
            }
            else
            {
                graphicModel.Values = AllValues.TakeLast(AllValues.Count());
                graphicModel.Labels = FillGraphicLabels(AllValues.Count());
                return Json(graphicModel);
            }    
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
                List<TestDataJson> testDataJson = new List<TestDataJson>();
                var metrics = _db.Metrics.ToList();
                foreach (var item in metrics)
                {
                    testDataJson.Add(new TestDataJson { Name = item.Name, IsBoolean = item.IsBoolean, Priority = item.Priority, Kind = item.Kind, WarningThreshold = item.WarningThreshold, AlarmThreshold = item.AlarmThreshold, Value = rnd.Next(0, item.AlarmThreshold + (item.AlarmThreshold - item.WarningThreshold)) });
                }
                return Json(testDataJson[rnd.Next(0, metrics.Count())]);
            }
            else
            {
                List<TestDataJson> testDataJson = new List<TestDataJson>
                {
                   new TestDataJson { Name = "Name3", IsBoolean = false, WarningThreshold = 30, AlarmThreshold = 120, Priority = PriorityKind.Medium, Kind = "Kind3", Value = rnd.Next(0, 150) },
                   new TestDataJson { Name = "Name2", IsBoolean = false, WarningThreshold = 60, AlarmThreshold = 90, Priority = PriorityKind.High, Kind = "Kind2", Value = rnd.Next(0, 120) },
                   new TestDataJson { Name = "Name1", IsBoolean = false, WarningThreshold = 5, AlarmThreshold = 12, Priority = PriorityKind.Low, Kind = "Kind1", Value = rnd.Next(0, 30) },
                   new TestDataJson { Name = "Name4", IsBoolean = false, WarningThreshold = 20, AlarmThreshold = 30, Priority = PriorityKind.High, Kind = "Kind4", Value = rnd.Next(0, 40) }
                };
                return Json(testDataJson[rnd.Next(0, 4)]);
            }
        }

        /// <summary>
        /// Обработка принятых данных. Вследствие чего, добавляются новые метрики,
        /// которых нет в базе данных и добавляются логи в базу данных, если метрика уже существовала в базе даннах.
        /// </summary>
        /// <param name="JsonData">Принятые JSON данные из представления.</param>
        /// <returns>Возвращает код состояния.</returns>
        [HttpPost]
        public IActionResult ProcessData([FromBody]JsonElement JsonData)
        {
            JsonConverters jsonConverters = new JsonConverters();
            var Data = jsonConverters.DeserializeTestData(JsonData);
            if (Data != null)
            {
                if (_db.Metrics.Select(i => i.Kind).Contains(Data.Kind))
                {
                    LogObject NewLog = new LogObject
                    {
                        MetricId = _db.Metrics.Where(i => i.Kind == Data.Kind).Select(i => i.Id).First(),
                        Date = DateTime.Now,
                        Value = Data.Value
                    };
                    _db.Logs.Add(NewLog);
                    _db.SaveChanges();
                    logger.Info($"Log saved! ({Data.Kind})");
                    return Ok($"Log saved! ({Data.Kind})");
                }
                else
                {
                    MetricItem NewMetric = new MetricItem
                    {
                        Name = "",
                        IsBoolean = Data.IsBoolean,
                        WarningThreshold = Data.WarningThreshold,
                        AlarmThreshold = Data.AlarmThreshold,
                        Priority = Data.Priority,
                        Kind = Data.Kind
                    };
                    _db.Metrics.Add(NewMetric);
                    _db.SaveChanges();
                    LogObject NewLog = new LogObject
                    {
                        MetricId = _db.Metrics.Select(i => i.Id).ToList().Last(),
                        Date = DateTime.Now,
                        Value = Data.Value
                    };
                    _db.Logs.Add(NewLog);
                    _db.SaveChanges();
                    logger.Info($"New metric created and that metric's log saved! ({Data.Kind})");
                    return Ok($"New metric created and that metric's log saved! ({Data.Kind})");
                }
            }
            return StatusCode(500, "Произошла ошибка при десериализации");
        }

        /// <summary>
        /// Заполнение ярлыков для графика.
        /// </summary>
        /// <param name="ConditionValue">Устанавливает кол-во ярлыков.</param>
        /// <returns>Возвращает список ярлыков</returns>       
        private List<int> FillGraphicLabels(int ConditionValue)
        {
            List<int> labels = new List<int>();
            for (int i = 0; i < ConditionValue; i++)
            {
                labels.Add(i + 1);
            };
            return labels;
        }
    }
}
