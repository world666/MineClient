using System;
using System.Configuration;
using System.Globalization;
using WpfClient.Model.Abstract;
using WpfClient.Model.Settings;

namespace WpfClient.Services
{
    class MineConfig : IConfig
    {
        private readonly Configuration _config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        private FanObjectConfigSection _fanObjectConfig;

        private double _maxTemperature;
        private double _maxIndicatorValue;
        private double _parameterWarning;
        private double _parameterDanger;
        private double _maxPillowValue;

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
            get { return (_maxTemperature = getParameter("MaxTemperature", _maxTemperature)); }
            set { setParameter("MaxTemperature", ref _maxTemperature, value); }
        }

        public double MaxPillowValue
        {
            get { return (_maxPillowValue = getParameter("MaxPillowValue", _maxPillowValue)); } 
            set { setParameter("MaxPillowValue", ref _maxPillowValue, value); }
        }

        public double MaxIndicatorValue 
        {
            get { return (_maxIndicatorValue = getParameter("MaxIndicatorValue", _maxIndicatorValue)); }
            set { setParameter("MaxIndicatorValue", ref _maxIndicatorValue, value); }
        }

        public double ParameterWarning
        {
            get { return (_parameterWarning = getParameter("ParameterWarning", _parameterWarning)); }
            set { setParameter("ParameterWarning", ref _parameterWarning, value); }
        }

        public double ParameterDanger 
        {
            get { return (_parameterDanger = getParameter("ParameterDanger", _parameterDanger)); }
            set { setParameter("ParameterDanger", ref _parameterDanger, value); }
        }

        private double getParameter(string name, double value)
        {
            if (Math.Abs(value) < 1e-10)
            {
                value = double.Parse(ConfigurationManager.AppSettings[name]);
            }
            return value;
        }

        private void setParameter(string name, ref double oldValue, double newValue)
        {
            oldValue = 0;
            ConfigurationManager.AppSettings[name] = newValue.ToString(CultureInfo.InvariantCulture);
        }
    }
}
