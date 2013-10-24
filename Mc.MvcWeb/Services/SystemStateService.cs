using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using Mc.MvcWeb.Models.Index;
using Mc.Settings.Model.Concrete;

namespace Mc.MvcWeb.Services
{
    public class SystemStateService
    {
        public static StateEnum GetParameterState(string name, double value)
        {
            double warningValue = 100;
            double dangerValue = 100;

            if (name.Contains("Расход"))//сравнение на меньше
            {
                warningValue = -Config.Instance.FanObjectConfig.AirConsumption.WarningLevel;
                dangerValue = -Config.Instance.FanObjectConfig.AirConsumption.DangerLevel;
                value = -value;
            }
            else if (name.Contains("Депрессия"))//сравнение на меньше
            {
                warningValue = -Config.Instance.FanObjectConfig.Pressure.WarningLevel;
                dangerValue = -Config.Instance.FanObjectConfig.Pressure.DangerLevel;
                value = -value;
            }
            else if (name.Contains("Температура"))//сравнение на больше
            {
                warningValue = Config.Instance.FanObjectConfig.PillowTemperature.WarningLevel;
                dangerValue = Config.Instance.FanObjectConfig.PillowTemperature.DangerLevel;
            }
            else if (name.Contains("Вибрация"))//сравнение на больше
            {
                warningValue = Config.Instance.FanObjectConfig.PillowVibration.WarningLevel;
                dangerValue = Config.Instance.FanObjectConfig.PillowVibration.DangerLevel;
            }
            else if (name.Contains("Ток статора"))//сравнение на меньше
            {
                warningValue = Config.Instance.FanObjectConfig.StatorCurrent.WarningLevel;
                dangerValue = Config.Instance.FanObjectConfig.StatorCurrent.DangerLevel;
            }
            else if (name.Contains("Ток ротора"))//сравнение на двойной интервал
            {
                if (value < Config.Instance.FanObjectConfig.RotorCurrentHigh.WarningLevel)
                {
                    warningValue = -Config.Instance.FanObjectConfig.RotorCurrentLow.WarningLevel;
                    dangerValue = -Config.Instance.FanObjectConfig.RotorCurrentLow.DangerLevel;
                    value = -value;
                }
                else
                {
                    warningValue = Config.Instance.FanObjectConfig.RotorCurrentHigh.WarningLevel;
                    dangerValue = Config.Instance.FanObjectConfig.RotorCurrentHigh.DangerLevel;
                }
            }
            else if (name.Contains("Проток"))//сравнение на меньше
            {
                warningValue = -Config.Instance.FanObjectConfig.OilFlow.WarningLevel;
                dangerValue = -Config.Instance.FanObjectConfig.OilFlow.DangerLevel;
                value = -value;
            }
            else if (name.Contains("Качество"))//сравнение на меньше
            {
                warningValue = -Config.Instance.FanObjectConfig.GprsQuality.WarningLevel;
                dangerValue = -Config.Instance.FanObjectConfig.GprsQuality.DangerLevel;
                value = -value;
            }
            if (value <= warningValue) return StateEnum.Ok;

            return value > dangerValue ? StateEnum.Dangerous : StateEnum.Warning;
        }

        public static StateEnum GetFanState(int workingFan)
        {
            return workingFan == 0 ? StateEnum.Dangerous : StateEnum.Ok;
        }

        public static double GetLinearAnalogValue(string name, double value)
        {
            if (name.Contains("Расход"))//сравнение на меньше
            {
                return value * Config.Instance.AirFlowСoefficient;
            }
            else if (name.Contains("Депрессия"))//сравнение на меньше
            {
                return value * Config.Instance.PressureСoefficient;
            }
            else if (name.Contains("Температура"))//сравнение на больше
            {
                return value * Config.Instance.TemperatureСoefficient;
            }
            else if (name.Contains("Вибрация"))//сравнение на больше
            {
                return value * Config.Instance.PillowСoefficient;
            }
            return value;
        }
        public static double GetMaximumValue(string name)
        {
            if (name.Contains("Расход"))//сравнение на меньше
            {
                return Config.Instance.MaxAirFlowValue;
            }
            else if (name.Contains("Депрессия"))//сравнение на меньше
            {
                return Config.Instance.MaxPressureValue;
            }
            else if (name.Contains("Температура"))//сравнение на больше
            {
                return Config.Instance.MaxTemperature;
            }
            else if (name.Contains("Вибрация"))//сравнение на больше
            {
                return Config.Instance.MaxPillowValue;
            }
            else if (name.Contains("Качество"))//сравнение на больше
            {
                return Config.Instance.MaxSignalQualityValue;
            }
            else if (name.Contains("Скорость"))//сравнение на больше
            {
                return Config.Instance.MaxSpeedValue;
            }
            else if (name.Contains("Ток"))//сравнение на больше
            {
                return Config.Instance.MaxCurrentValue;
            }
            else if (name.Contains("Проток"))//сравнение на больше
            {
                return Config.Instance.MaxOilFlowValue;
            }
            return 0;
        }
        public static Parameter checkRemoteSignalState(DateTime dateTime)
        {
            Parameter SignalState = new Parameter();
            SignalState.Name = "Состояние сигнала";
            if (System.DateTime.Now - dateTime > new TimeSpan(0, 1, 0))
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

        public static Parameter getFanStateParameter(FanObject fanObject)
        {
            return GetFanMode(fanObject.WorkingFanNumber, fanObject.Doors);
        }

        public static Parameter getFanNumberParameter(FanObject fanObject)
        {
            var parameter = new Parameter { Value = fanObject.WorkingFanNumber == 0 ? "ОСТАНОВЛЕН" : string.Format("№{0}", fanObject.WorkingFanNumber), Maximum = "0", Name = "Вентилятор в работе" };
            parameter.State = GetFanState(fanObject.WorkingFanNumber);

            return parameter;
        }
        private static Parameter GetFanMode(int workingFanNum, List<Door> doors)
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