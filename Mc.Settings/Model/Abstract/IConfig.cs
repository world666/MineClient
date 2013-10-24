using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mc.Settings.Model.Settings;

namespace Mc.Settings.Model.Abstract
{
    public interface IConfig
    {
        double MaxTemperature { get; set; }

        double MaxPillowValue { get; set; }

        double MaxAirFlowValue { get; set; }

        double MaxPressureValue { get; set; }

        double MaxSignalQualityValue { get; set; }

        double TemperatureСoefficient { get; set; }

        double MaxSpeedValue { get; set; }

        double MaxCurrentValue { get; set; }

        double MaxOilFlowValue { get; set; }

        double PillowСoefficient { get; set; }

        double AirFlowСoefficient { get; set; }

        double PressureСoefficient { get; set; }

        string RemotePassword { get; set; }

        FanObjectConfigSection FanObjectConfig { get; }
    }
}
