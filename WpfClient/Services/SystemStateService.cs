using System;
using WpfClient.Model;
using WpfClient.Model.Concrete;
using WpfClient.Model.Entities;
using WpfClient.Model.Settings;

namespace WpfClient.Services
{
    class SystemStateService
    {
        public static StateEnum GetParameterState(string name,int value)
        {
            double warningValue = Config.Instance.ParameterWarning;
            double dangerValue = Config.Instance.ParameterDanger;

            if(name.Contains("Расход"))//сравнение на меньше
            {
                warningValue = -Config.Instance.FanObjectConfig.AirConsumption.WarningLevel;
                dangerValue = -Config.Instance.FanObjectConfig.AirConsumption.DangerLevel;
                value = -value;
            }
            else if (name.Contains("Давление"))//сравнение на меньше
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

        
    }
}
