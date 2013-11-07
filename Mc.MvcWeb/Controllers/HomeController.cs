using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DataRepository.DataAccess.GenericRepository;
using DataRepository.Models;
using Mc.MvcWeb.Models.Control;
using Mc.MvcWeb.Models.Index;
using Mc.MvcWeb.Services;
using Mc.Settings.Model.Concrete;

namespace Mc.MvcWeb.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        DatabaseService _databaseService = new DatabaseService();
        public ActionResult Index()
        {
            List<Fan> _fans = FanService.getFans();
            ViewBag.Fans = _fans;
            ViewBag.MineName = Config.Instance.FanObjectConfig.MineName;
            ViewBag.Date = DateTime.Now;
            ViewBag.ParametersCount = _fans[0].Values.Count;
            List<string> fanNames = Config.Instance.FanObjectConfig.FansName.ToList();  
            if (fanNames.Count() < _fans.Count())
            {
                for (int i = 0; i < _fans.Count() - fanNames.Count(); i++)
                {
                    fanNames.Add(fanNames.Count().ToString());
                }
            }
            ViewBag.FanNamesCount = _fans.Count();
            ViewBag.FanNames = fanNames;
            return View();
        }
        public ActionResult IndexContent()
        {
            List<Fan> _fans = FanService.getFans();
            ViewBag.Fans = _fans;
            ViewBag.MineName = Config.Instance.FanObjectConfig.MineName;
            ViewBag.Date = DateTime.Now;
            ViewBag.ParametersCount = _fans[0].Values.Count;
            List<string> fanNames = Config.Instance.FanObjectConfig.FansName.ToList();
            if (fanNames.Count() < _fans.Count())
            {
                for (int i = 0; i < _fans.Count() - fanNames.Count(); i++)
                {
                    fanNames.Add(fanNames.Count().ToString());
                }
            }
            ViewBag.FanNamesCount = _fans.Count();
            ViewBag.FanNames = fanNames;
            return View();
        }
        [HttpGet]
        public ActionResult Control(int fanObjectId)
        {
            var fanObject = _databaseService.GetFanObject(fanObjectId);
            if (fanObject == null) return null;
            ViewBag.FanObject = fanObject;
            ViewBag.FanName = Config.Instance.FanObjectConfig.FansName[fanObjectId - 1];
            ViewBag.DoorsState = FanService.GetDoorsState(fanObject.Doors);
            ViewBag.DoorsMode = FanService.GetDoorsMode(fanObject.WorkingFanNumber, fanObject.Doors);
            ViewBag.DoorsTextColor = FanService.GetDoorsTextColor(fanObject.Doors);

            return View();
        }
        [HttpPost]
        public ActionResult Control(RemoteData remoteData)
        {
            if (remoteData.Password == Config.Instance.RemotePassword)
            {
                RemoteLog remoteLog = new RemoteLog
                    {
                        FanObjectNum = remoteData.FanObjectId,
                        Date = DateTime.Now,
                        Person = "Веб-сервер",
                        RemoteStateId = remoteData.Command,
                        Sent = false
                    };
                using (var repoUnit = new RepoUnit())
                {
                    repoUnit.RemoteLog.Add(remoteLog);
                    repoUnit.RemoteLog.SaveChanges();
                }
            }
            return Redirect(Request.UrlReferrer.ToString());
        }

        public ActionResult ControlContent(int fanObjectId)
        {
            var fanObject = _databaseService.GetFanObject(fanObjectId);
            if (fanObject == null) return null;
            ViewBag.FanObject = fanObject;
            ViewBag.FanName = Config.Instance.FanObjectConfig.FansName[fanObjectId - 1];
            ViewBag.DoorsState = FanService.GetDoorsState(fanObject.Doors);
            ViewBag.DoorsMode = FanService.GetDoorsMode(fanObject.WorkingFanNumber, fanObject.Doors);
            ViewBag.DoorsTextColor = FanService.GetDoorsTextColor(fanObject.Doors);
            return View();
        }
    }
}
