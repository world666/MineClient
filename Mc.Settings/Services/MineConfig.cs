using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mc.Settings.Model.Abstract;
using Mc.Settings.Model.Settings;

namespace Mc.Settings.Services
{
    class MineConfig : IConfig
    {
        private readonly Configuration _config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        private FanObjectConfigSection _fanObjectConfig;

        private double _maxTemperature;
        private double _maxIndicatorValue;
        private double _maxPillowValue;
        private double _maxAirFlowValue;
        private double _maxPressureValue;

        public FanObjectConfigSection FanObjectConfig
        {
            get { return _fanObjectConfig ?? (_fanObjectConfig = (FanObjectConfigSection)_config.GetSection("FanObjectConfig")); }
        }

        public void Save()
        {
            _config.Save(ConfigurationSaveMode.Full);
        }


        public double MaxTemperature
        {
            get { return (_maxTemperature = getParameter("MaxTemperature")); }
            set { setParameter("MaxTemperature", value); }
        }

        public double MaxPillowValue
        {
            get { return (_maxPillowValue = getParameter("MaxPillowValue")); }
            set { setParameter("MaxPillowValue", value); }
        }

        public double MaxAirFlowValue
        {
            get { return (_maxAirFlowValue = getParameter("MaxAirFlowValue")); }
            set { setParameter("MaxAirFlowValue", value); }
        }

        public double MaxPressureValue
        {
            get { return (_maxPressureValue = getParameter("MaxPressureValue")); }
            set { setParameter("MaxPressureValue", value); }
        }

        public double MaxSignalQualityValue
        {
            get { return (getParameter("MaxSignalQualityValue")); }
            set { setParameter("MaxSignalQualityValue", value); }
        }

        public double TemperatureСoefficient
        {
            get { return (getParameter("TemperatureСoefficient")); }
            set { setParameter("TemperatureСoefficient", value); }
        }

        public double AirFlowСoefficient
        {
            get { return (getParameter("AirFlowСoefficient")); }
            set { setParameter("AirFlowСoefficient", value); }
        }

        public double PressureСoefficient
        {
            get { return (getParameter("PressureСoefficient")); }
            set { setParameter("PressureСoefficient", value); }
        }

        public double PillowСoefficient
        {
            get { return (getParameter("PillowСoefficient")); }
            set { setParameter("PillowСoefficient", value); }
        }

        public string RemotePassword
        {
            get
            {
                int value = Int32.Parse(ConfigurationManager.AppSettings["RemotePassword"]);
                return (value - 1132).ToString();
            }
            set { setParameter("RemotePassword", value); }
        }

        private double getParameter(string name)
        {

            double value = double.Parse(ConfigurationManager.AppSettings[name]);

            return value;
        }
        private void setParameter(string configKey, double value)
        {

            //make changes
            _config.AppSettings.Settings[configKey].Value = value.ToString();

            _config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }
        private void setParameter(string configKey, string value)
        {
            //make changes
            _config.AppSettings.Settings[configKey].Value = value;
            _config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }
    }
}
