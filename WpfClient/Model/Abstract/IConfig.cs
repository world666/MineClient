using WpfClient.Model.Settings;

namespace WpfClient.Model.Abstract
{
    public interface IConfig
    {
        double MaxTemperature { get; set; }

        double MaxPillowValue { get; set; }

        double MaxAirFlowValue { get; set; }

        double MaxPressureValue { get; set; }

        double Temperature—oefficient { get; set; }

        double Pillow—oefficient { get; set; }

        double AirFlow—oefficient { get; set; }

        double Pressure—oefficient { get; set; }

        FanObjectConfigSection FanObjectConfig { get; }

        void Save();
    }
}
