using System;
using Mc.Settings.Model.Concrete;
using WpfClient.Model;
using WpfClient.Model.Concrete;
using WpfClient.Model.Entities;
using WpfClient.Model.Settings;

namespace WpfClient.Services
{
    class SystemStateService
    {
        public static StateEnum GetParameterState(string name,double value)
        {
            double warningValue = 100;
            double dangerValue = 100;

            if(name.Contains("Расход"))//сравнение на меньше
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
                return value* Config.Instance.AirFlowСoefficient;
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
            return 0;
        }     
    }
}
