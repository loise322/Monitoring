using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Monitoring.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Monitoring.Controllers
{
    public class HomeController : Controller
    {

        TableContext _db;
        public HomeController(TableContext context)
        {
            _db = context;
        }
        public IActionResult Index()
        {
            var vocs = _db.Vocs;
            var logs = _db.Logs;
            int value = (from i in _db.Logs select i.Value).ToList().Last();
            ViewBag.Vocs = vocs;
            ViewBag.Logs = logs;
            ViewBag.Value = value.ToString();
            return View();
        } 
        public IActionResult Update()
        {
            Random rnd = new Random();

            LogObject nLog = new LogObject
            {
                Value = rnd.Next(0, 12),
                Date = DateTime.Now.ToShortDateString()
            };

            _db.Logs.Add(nLog);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
