using WpfClient.Model.Settings;

namespace WpfClient.Model.Abstract
{
    public interface IConfig
    {
        double MaxTemperature { get; set; }

        double MaxPillowValue { get; set; }

        double MaxIndicatorValue { get; set; }

        double ParameterWarning { get; set; }

        double ParameterDanger { get; set; }

        FanObjectConfigSection FanObjectConfig { get; }

        void Save();
    }
}
