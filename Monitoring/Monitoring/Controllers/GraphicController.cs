using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Monitoring.Services;

namespace Monitoring.Controllers
{
    public class GraphicController : Controller
    {
        private readonly IProcessingGraphic _processingGraphic;

        public GraphicController (IProcessingGraphic processingGraphic)
        {
            _processingGraphic = processingGraphic;
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
    }
}
