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
        public static double GetCoefficient(string name)
        {
            if (name.Contains("Расход"))//сравнение на меньше
            {
                return Config.Instance.AirFlowСoefficient;
            }
            else if (name.Contains("Депрессия"))//сравнение на меньше
            {
                return Config.Instance.PressureСoefficient;
            }
            else if (name.Contains("Температура"))//сравнение на больше
            {
                return Config.Instance.TemperatureСoefficient;
            }
            else if (name.Contains("Вибрация"))//сравнение на больше
            {
                return  Config.Instance.PillowСoefficient;
            }
            return 1;
        }
        public static double GetMaximumValue(string name)
        {
            if (name.Contains("Расход"))//сравнение на меньше
            {
                return Config.Instance.MaxAirFlowValue;
            }
            else if (name.Contains("Депрессия") || name.Contains("Давление"))//сравнение на меньше
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
    }
}
