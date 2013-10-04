using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mc.MvcWeb.Models.Index;
using Mc.Settings.Model.Concrete;

namespace Mc.MvcWeb.Services
{
    public static class FanService
    {
        public static List<Fan> getFans()
        {
            List<Fan> _fans = new List<Fan>();
            DatabaseService _databaseService = new DatabaseService();

            var parametersList = new List<List<Parameter>>();

            for (var i = 1; i <= Config.Instance.FanObjectConfig.FanObjectCount; i++)
            {
                var fanObject = _databaseService.GetFanObject(i);
                if (fanObject == null) continue;
                _fans.Add(new Fan(i));
                parametersList.Add(new List<Parameter>());

                parametersList[i - 1].Add(SystemStateService.checkRemoteSignalState(fanObject.Date));
                parametersList[i - 1].Add((SystemStateService.getFanNumberParameter(fanObject)));
                parametersList[i - 1].Add((SystemStateService.getFanStateParameter(fanObject)));
                fanObject.Parameters.ForEach(p => parametersList[i - 1].Add(p));
            }
            for (var i = 0; i < parametersList.Count; i++) _fans[i].Values = parametersList[i];
            return _fans;
        }
    }
}