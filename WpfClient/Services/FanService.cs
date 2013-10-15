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
            var state = mode.Contains("АВАРИЯ")
                            ? StateEnum.Dangerous : mode.Contains("Реверс")
                            ? StateEnum.Warning
                            : StateEnum.Ok;

            return new ParameterVm
            {
                Name = "Состояние вентилятора",
                Value = mode,
                State =  state, 
                Maximum = "0"
            };
        }

        private string getModeString(int workingFanNum, string pattern)
        {
            if (workingFanNum == 1) 
            {
                return pattern == "332223332" ? "Норма" :
                       pattern == "332232232" ? "Реверс" : "АВАРИЯ (Ляды не собраны)";
            } 
            if (workingFanNum == 2) 
            {
               return  pattern == "223323323" ? "Норма" :
                       pattern == "223332223" ? "Реверс" : "АВАРИЯ (Ляды не собраны)";
            }
            return "АВАРИЯ (Вентилятор остановлен)";
        }
        public ParameterVm getFanNumberParameter(FanObject fanObject)
        {
            var parameter = new ParameterVm { Value = fanObject.WorkingFanNumber == 0 ? "ОСТАНОВЛЕН" : string.Format("№{0}", fanObject.WorkingFanNumber), Maximum = "0", Name = "Вентилятор в работе" };
            parameter.State = SystemStateService.GetFanState(fanObject.WorkingFanNumber);

            return parameter;
        }
        public ParameterVm checkRemoteSignalState(int fanObjectId)
        {
            ParameterVm SignalState = new ParameterVm();
            SignalState.Name = "Состояние сигнала";
            if (System.DateTime.Now - RemoteService.GetLastRecieve(fanObjectId) > new TimeSpan(0, 1, 0))
            {
                SignalState.Value = "отсутствует";
                SignalState.State = StateEnum.Dangerous;
            }
            else
            {
                SignalState.Value = "стабильный";
                SignalState.State = StateEnum.Ok;
            }
            SignalState.Maximum = "0";
            return SignalState;
        }
    }
}
