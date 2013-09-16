using System;
using System.Collections.Generic;
using System.Text;
using WpfClient.Model;
using WpfClient.Model.Entities;
using WpfClient.ViewModel;

namespace WpfClient.Services
{
    public class FanService
    {
        public ParameterVm GetFanMode(int workingFanNum, List<Door> doors)
        {
            var pattern = new StringBuilder();
            doors.ForEach(d => pattern.Append(d.StateId));

            var mode = getModeString(workingFanNum, pattern.ToString());
            var state = mode.Equals("АВАРИЯ", StringComparison.InvariantCultureIgnoreCase)
                            ? StateEnum.Dangerous : mode.Equals("Реверс", StringComparison.InvariantCultureIgnoreCase)
                            ? StateEnum.Warning
                            : StateEnum.Ok;

            return new ParameterVm
            {
                Name = "Состояние вентилятора",
                Value = mode,
                State =  state 
            };
        }

        private string getModeString(int workingFanNum, string pattern)
        {
            if (workingFanNum == 1) 
            {
                return pattern == "332223332" ? "Норма" :
                       pattern == "332232232" ? "Реверс" : "АВАРИЯ";
            } 
            if (workingFanNum == 2) 
            {
               return  pattern == "223323323" ? "Норма" :
                       pattern == "223332223" ? "Реверс" : "АВАРИЯ";
            }
            return "АВАРИЯ";
        }
    }
}
