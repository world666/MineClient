using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Mc.CustomControls.Model;
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
        public static List<string> GetDoorsState(List<Door> doors)
        {
            List<string> states = new List<string>();
            foreach (var door in doors)
            {
                DoorStateEnum state = Enum.IsDefined(typeof(DoorStateEnum), door.StateId) ? (DoorStateEnum)door.StateId : DoorStateEnum.Undefined;
                string strState = "";
                switch (state)
                {
                    case DoorStateEnum.Open:
                        strState = "Opened";
                        break;
                    case DoorStateEnum.Close:
                        strState = "Closed";
                        break;
                    default:
                        strState = "Undefined";
                        break;
                }
                states.Add(strState);
                /*DoorsText[(int)type] = DoorsState[(int)type] == DoorStateEnum.Undefined
                                            ? "Red"
                               : "WhiteSmoke";*/
            }
            return states;
        }
        public static Parameter GetDoorsMode(int workingFanNum, List<Door> doors)
        {
            var pattern = new StringBuilder();
            doors.ForEach(d => pattern.Append(d.StateId));

            var mode = getModeString(workingFanNum, pattern.ToString());
            var state = mode.Equals("АВАРИЯ", StringComparison.InvariantCultureIgnoreCase)
                            ? StateEnum.Dangerous : mode.Equals("Реверс", StringComparison.InvariantCultureIgnoreCase)
                            ? StateEnum.Warning
                            : StateEnum.Ok;

            return new Parameter
            {
                Name = "Состояние вентилятора",
                Value = mode,
                State = state,
                Maximum = "0"
            };
        }

        private static string getModeString(int workingFanNum, string pattern)
        {
            if (workingFanNum == 1)
            {
                return pattern == "332223332" ? "Норма" :
                       pattern == "332232232" ? "Реверс" : "АВАРИЯ";
            }
            if (workingFanNum == 2)
            {
                return pattern == "223323323" ? "Норма" :
                        pattern == "223332223" ? "Реверс" : "АВАРИЯ";
            }
            return "АВАРИЯ";
        }
    
    }
   
}