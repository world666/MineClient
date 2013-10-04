using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataRepository.DataAccess.GenericRepository;
using Mc.Settings.Model.Abstract;
using Mc.Settings.Model.Concrete;
using Mc.Settings.Model.Settings;

namespace Mc.Settings.Services
{
    class MineConfig : IConfig
    {
         private FanObjectConfigSection _fanObjectConfig;
        public FanObjectConfigSection FanObjectConfig
        {
            get { return _fanObjectConfig ?? (_fanObjectConfig = new FanObjectConfigSection()); }
        }



        public double MaxTemperature
        {
            set
            {
                using (var repoUnit = new RepoUnit())
                {
                    var log = repoUnit.SettingsLog.FindFirstBy(f => f.Name == "MaxTemperature");
                    log.DValue = value;
                    repoUnit.SettingsLog.Edit(log);
                }
            }
            get
            {
                double value;
                using (var repoUnit = new RepoUnit())
                {
                     value = repoUnit.SettingsLog.FindFirstBy(f => f.Name == "MaxTemperature").DValue;
                }
                return value;
            }
        }

        public double MaxPillowValue
        {
            set
            {
                using (var repoUnit = new RepoUnit())
                {
                    var log = repoUnit.SettingsLog.FindFirstBy(f => f.Name == "MaxPillowValue");
                    log.DValue = value;
                    repoUnit.SettingsLog.Edit(log);
                }
            }
            get
            {
                double value;
                using (var repoUnit = new RepoUnit())
                {
                    value = repoUnit.SettingsLog.FindFirstBy(f => f.Name == "MaxPillowValue").DValue;
                }
                return value;
            }
        }

        public double MaxAirFlowValue
        {
            set
            {
                using (var repoUnit = new RepoUnit())
                {
                    var log = repoUnit.SettingsLog.FindFirstBy(f => f.Name == "MaxAirFlowValue");
                    log.DValue = value;
                    repoUnit.SettingsLog.Edit(log);
                }
            }
            get
            {
                double value;
                using (var repoUnit = new RepoUnit())
                {
                    value = repoUnit.SettingsLog.FindFirstBy(f => f.Name == "MaxAirFlowValue").DValue;
                }
                return value;
            }
        }

        public double MaxPressureValue
        {
            set
            {
                using (var repoUnit = new RepoUnit())
                {
                    var log = repoUnit.SettingsLog.FindFirstBy(f => f.Name == "MaxPressureValue");
                    log.DValue = value;
                    repoUnit.SettingsLog.Edit(log);
                }
            }
            get
            {
                double value;
                using (var repoUnit = new RepoUnit())
                {
                    value = repoUnit.SettingsLog.FindFirstBy(f => f.Name == "MaxPressureValue").DValue;
                }
                return value;
            }
        }

        public double MaxSignalQualityValue
        {
            set
            {
                using (var repoUnit = new RepoUnit())
                {
                    var log = repoUnit.SettingsLog.FindFirstBy(f => f.Name == "MaxSignalQualityValue");
                    log.DValue = value;
                    repoUnit.SettingsLog.Edit(log);
                }
            }
            get
            {
                double value;
                using (var repoUnit = new RepoUnit())
                {
                    value = repoUnit.SettingsLog.FindFirstBy(f => f.Name == "MaxSignalQualityValue").DValue;
                }
                return value;
            }
        }

        public double TemperatureСoefficient
        {
            set
            {
                using (var repoUnit = new RepoUnit())
                {
                    var log = repoUnit.SettingsLog.FindFirstBy(f => f.Name == "TemperatureСoefficient");
                    log.DValue = value;
                    repoUnit.SettingsLog.Edit(log);
                }
            }
            get
            {
                double value;
                using (var repoUnit = new RepoUnit())
                {
                    value = repoUnit.SettingsLog.FindFirstBy(f => f.Name == "TemperatureСoefficient").DValue;
                }
                return value;
            }
        }

        public double AirFlowСoefficient
        {
            set
            {
                using (var repoUnit = new RepoUnit())
                {
                    var log = repoUnit.SettingsLog.FindFirstBy(f => f.Name == "AirFlowСoefficient");
                    log.DValue = value;
                    repoUnit.SettingsLog.Edit(log);
                }
            }
            get
            {
                double value;
                using (var repoUnit = new RepoUnit())
                {
                    value = repoUnit.SettingsLog.FindFirstBy(f => f.Name == "AirFlowСoefficient").DValue;
                }
                return value;
            }
        }

        public double PressureСoefficient
        {
            set
            {
                using (var repoUnit = new RepoUnit())
                {
                    var log = repoUnit.SettingsLog.FindFirstBy(f => f.Name == "PressureСoefficient");
                    log.DValue = value;
                    repoUnit.SettingsLog.Edit(log);
                }
            }
            get
            {
                double value;
                using (var repoUnit = new RepoUnit())
                {
                    value = repoUnit.SettingsLog.FindFirstBy(f => f.Name == "PressureСoefficient").DValue;
                }
                return value;
            }
        }

        public double PillowСoefficient
        {
            set
            {
                using (var repoUnit = new RepoUnit())
                {
                    var log = repoUnit.SettingsLog.FindFirstBy(f => f.Name == "PillowСoefficient");
                    log.DValue = value;
                    repoUnit.SettingsLog.Edit(log);
                }
            }
            get
            {
                double value;
                using (var repoUnit = new RepoUnit())
                {
                    value = repoUnit.SettingsLog.FindFirstBy(f => f.Name == "PillowСoefficient").DValue;
                }
                return value;
            }
        }

        public string RemotePassword
        {
            set
            {
                using (var repoUnit = new RepoUnit())
                {
                    var log = repoUnit.SettingsLog.FindFirstBy(f => f.Name == "RemotePassword");
                    log.SValue = (Int32.Parse(value) + 1132).ToString();
                    repoUnit.SettingsLog.Edit(log);
                }
            }
            get
            {
                string value;
                using (var repoUnit = new RepoUnit())
                {
                    value = repoUnit.SettingsLog.FindFirstBy(f => f.Name == "RemotePassword").SValue;
                    value = (Int32.Parse(value) - 1132).ToString();
                }
                return value;
            }
        }
    }
}
