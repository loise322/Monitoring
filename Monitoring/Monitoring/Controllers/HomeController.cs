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
using Monitoring.ValidatorRepository;
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
        /// Метод Index() используется для отображения представления Index. 
        /// </summary>
        /// <returns>Возвращает представление Index.</returns>
        public IActionResult Index()
        {
            ViewBag.Title = "Monitoring";
            return View();
        }
        /// <summary>
        /// Метод Metrics() используется для отображения представления Metrics со списком всех метрик, которые есть в базе данных 
        /// </summary>
        /// <returns>Возвращает представление Metrics со списком всех метрик, которые есть в базе данных </returns>
        public IActionResult Metrics()
        {
            ViewBag.Title = "Monitoring";
            MetricsModel Model = new MetricsModel();
            Model.Metrics = _db.Metrics.ToList();
            return View(Model);
        }
        /// <summary>
        /// Метод Edit() используется для отображения представления Edit с метрикой, которую редактируем.
        /// </summary>
        /// <param name="id">Аргумент метода Edit(). Указывает на метрику, которую редактируем.</param>
        /// <returns>Возвращает представление с метрикой, которую редактируем</returns>
        public IActionResult Edit(int id)
        {
            ViewBag.Title = "Monitoring";
            MetricItem Metric = new MetricItem();  
            Metric = _db.Metrics.Where(i => i.Id == id).FirstOrDefault();
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
            else
            {
                return View();
            }
        }
        /// <summary>
        /// Метод EditMetric() используется для редактирование метрик в базе данных.
        /// </summary>
        /// <param name="Data">Аргумент метода AddMetric(). JSON данные.</param>
        /// <returns>Возвращает коды состояния и ошибки, которые произошли при валидации</returns>
        [HttpPost]
        public IActionResult EditMetric([FromBody]JsonElement Data)
        {
            Validator validator = new Validator();
            var DataForEdit = validator.DeserializationMetric(Data);
            if (DataForEdit != null)
            {
                var MetricForEdit = _db.Metrics.Where(i => i.Id == DataForEdit.Id).FirstOrDefault();
                var ValidationErrors = validator.StringsValidation(DataForEdit.Name, DataForEdit.Kind);
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
                    return BadRequest(JsonConvert.SerializeObject(ValidationErrors.ToList()));
                }
            }
            else
            {
                return StatusCode(500, "Произошла ошибка при сериализации");
            }
        }
        /// <summary>
        /// Метод Add() используется для отображения представления Add.
        /// </summary>
        /// <returns>Возвращаент представление Add</returns>
        public IActionResult Add()
        {
            return View();
        }
        /// <summary>
        /// Метод AddMetric() используется для добавления метрик в базу данных.
        /// </summary>
        /// <param name="Data">Аргумент метода AddMetric(). JSON данные.</param>
        /// <returns>Возвращает коды состояния и ошибки, которые произошли при валидации</returns>
        [HttpPost]
        public IActionResult AddMetric([FromBody]JsonElement Data)
        {
            Validator validator = new Validator();
            var DataForAdd = validator.DeserializationMetric(Data);
            if (DataForAdd != null)
            {
                var ValidationErrors = validator.StringsValidation(DataForAdd.Name, DataForAdd.Kind);
                if (ValidationErrors.Count() == 0)
                {
                    _db.Metrics.Add(DataForAdd);
                    _db.SaveChanges();
                    logger.Info($"Metric added! {DataForAdd}");
                    return Ok();
                }
                else
                {
                    return BadRequest(JsonConvert.SerializeObject(ValidationErrors.ToList()));
                }
            }
            else
            {
                return StatusCode(500, "Произошла ошибка при сериализации");
            }
        }
        /// <summary>
        /// Метод Delete() используется для удаление метрик из базы данных
        /// </summary>
        /// <param name="id">Аргумент метода Delete(). Указывает на метрику, которую нужно удалить.</param>
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
        /// Метод Graphic() используется для отображения представления Graphic.
        /// </summary>
        /// <param name="id">Аргумент метода Graphic(). Указывает на метрику, график которой нужно построить</param>
        /// <returns>Возвращает представление Graphic</returns>
        public IActionResult Graphic(int id)
        {
            return View(new GraphicModel { MetricId = id });
        }
        /// <summary>
        /// Метод DataForGraphic() используется для отправки данных, с помощью которые строится график.
        /// </summary>
        /// <param name="id">Аргумент метода DataForGraphic(). Указывает на метрику, график которой нужно построить</param>
        /// <returns>Возвращает JSON массив данных</returns>
        [HttpGet]
        public IActionResult DataForGraphic(int id)
        {
            var AllValues = _db.Logs.Where(i => i.MetricId == id).Select(i => i.Value).ToList();
            const int MaxQuantity = 50;
            MethodsForGraphic methodsForGraphic = new MethodsForGraphic();
            GraphicModel graphicModel = new GraphicModel();
            if (AllValues.Count() > MaxQuantity)
            {
               
                graphicModel.Values = AllValues.TakeLast(MaxQuantity);
                graphicModel.Labels = methodsForGraphic.FillLabels(MaxQuantity);
                return Json(graphicModel);
            }
            else
            {
                graphicModel.Values = AllValues.TakeLast(AllValues.Count());
                graphicModel.Labels = methodsForGraphic.FillLabels(AllValues.Count());
                return Json(graphicModel);
            }    
        }
        /// <summary>
        /// Метод AcceptRequest() используется как источник данных для тестирования программы.
        /// </summary>
        /// <returns>Возвращает JSON данные в виде Name = string, IsBoolean = bool, WarningThreshold = int, AlarmThreshold = int, Priority = enum, Kind = string, Value = int</returns>
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
        /// Метод ProcessData() используется для обработки принятых данных. Вследствие чего, добавляются новые метрики,
        /// которых нет в базе данных и добавляются логи в базу данных, если метрика уже существовала в базе даннах.
        /// </summary>
        /// <param name="JsonData">Аргумент метода ProcessData(). JSON данные.</param>
        /// <returns>Возвращает код состояния.</returns>
        [HttpPost]
        public IActionResult ProcessData([FromBody]JsonElement JsonData)
        {
            Validator validator = new Validator();
            var Data = validator.DeserializationRequest(JsonData);
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
            else
            {
                return StatusCode(500, "Произошла ошибка при сериализации");
            }
        }
    }
}
