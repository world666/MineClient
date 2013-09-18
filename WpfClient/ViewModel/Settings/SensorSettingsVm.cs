using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClient.Model.Concrete;
using WpfClient.Services;

namespace WpfClient.ViewModel.Settings
{
    class SensorSettingsVm
    {
        public double MaxAirFlowText
        {
            get { return Config.Instance.MaxAirFlowValue; }
            set { Config.Instance.MaxAirFlowValue = value; }
        }
        public double MaxPressureText
        {
            get { return Config.Instance.MaxPressureValue; }
            set { Config.Instance.MaxPressureValue = value; }
        }
        public double MaxTemperatureText
        {
            get { return Config.Instance.MaxTemperature; }
            set { Config.Instance.MaxTemperature = value; }
        }
        public double MaxPillowText
        {
            get { return Config.Instance.MaxPillowValue; }
            set { Config.Instance.MaxPillowValue = value; }
        }
        public double TemperatureСoefficientText
        {
            get { return Config.Instance.TemperatureСoefficient; }
            set { Config.Instance.TemperatureСoefficient = value; }
        }
        public double AirFlowСoefficientText
        {
            get { return Config.Instance.AirFlowСoefficient; }
            set { Config.Instance.AirFlowСoefficient = value; }
        }
        public double PillowСoefficientText
        {
            get { return Config.Instance.PillowСoefficient; }
            set { Config.Instance.PillowСoefficient = value; }
        }
        public double PressureСoefficientText
        {
            get { return Config.Instance.PressureСoefficient; }
            set { Config.Instance.PressureСoefficient = value; }
        }
    }
}
