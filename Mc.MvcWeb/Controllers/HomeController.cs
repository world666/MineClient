using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mc.MvcWeb.Models;
using Mc.MvcWeb.Services;
using Mc.Settings.Model.Concrete;

namespace Mc.MvcWeb.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        List<Fan> _fans = new List<Fan>();
        DatabaseService _databaseService = new DatabaseService();
        public ActionResult Index()
        { 
            for (int i = 1; i <= Config.Instance.FanObjectConfig.FanObjectCount; i++)
            {
                _fans.Add(new Fan(i));
            }

            var parametersList = new List<List<Parameter>>();

            for (var i = 1; i <= _fans.Count; i++)
            {
                var fanObject = _databaseService.GetFanObject(i);
                if (fanObject == null) continue;

                parametersList.Add(new List<Parameter>());

                parametersList[i - 1].Add(SystemStateService.checkRemoteSignalState(fanObject.Date));
                parametersList[i - 1].Add((SystemStateService.getFanNumberParameter(fanObject)));
                parametersList[i - 1].Add((SystemStateService.getFanStateParameter(fanObject)));
                fanObject.Parameters.ForEach(p => parametersList[i - 1].Add(p));
            }
            for (var i = 0; i < parametersList.Count; i++) _fans[i].Values = parametersList[i];
            ViewBag.Fans = _fans;
            ViewBag.MineName = Config.Instance.FanObjectConfig.MineName;
            ViewBag.Date = DateTime.Now;
            string[] fanNames = Config.Instance.FanObjectConfig.FansName.Split(new string[] { "!$!" }, StringSplitOptions.RemoveEmptyEntries);
            ViewBag.FanNames = fanNames;
            ViewBag.FanNamesCount = fanNames.Count();
            ViewBag.ParametersCount = _fans[0].Values.Count;
            return View();
        }

        [HttpGet]
        public ActionResult Control(int fanObjectId)
        {
            var fanObject = _databaseService.GetFanObject(fanObjectId);
            if (fanObject == null) return null;
            ViewBag.FanObject = fanObject;
            ViewBag.FanName = Config.Instance.FanObjectConfig.FansName.Split(new string[] { "!$!" }, StringSplitOptions.RemoveEmptyEntries)[fanObjectId - 1];
            return View();
        }
    }
}
