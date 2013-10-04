using System;
using System.Configuration;
using DataRepository.DataAccess.GenericRepository;

namespace Mc.Settings.Model.Settings
{
    public class FanObjectConfigSection
    {
        public Int32 FanObjectCount
        {
            set
            {
                using (var repoUnit = new RepoUnit())
                {
                    var log = repoUnit.SettingsLog.FindFirstBy(f => f.Name == "FanObjectCount");
                    log.DValue = value;
                    repoUnit.SettingsLog.Edit(log);
                }
            }
            get
            {
                int value;
                using (var repoUnit = new RepoUnit())
                {
                    value = (int)repoUnit.SettingsLog.FindFirstBy(f => f.Name == "FanObjectCount").DValue;
                }
                return value;
            }
        }

        public string MineName
        {
            set
            {
                using (var repoUnit = new RepoUnit())
                {
                    var log = repoUnit.SettingsLog.FindFirstBy(f => f.Name == "mineName");
                    log.SValue = value;
                    repoUnit.SettingsLog.Edit(log);
                }
            }
            get
            {
                string value;
                using (var repoUnit = new RepoUnit())
                {
                    value = repoUnit.SettingsLog.FindFirstBy(f => f.Name == "mineName").SValue;
                }
                return value;
            }
        }

        public string FansName
        {
            set
            {
                using (var repoUnit = new RepoUnit())
                {
                    var log = repoUnit.SettingsLog.FindFirstBy(f => f.Name == "fansName");
                    log.SValue = value;
                    repoUnit.SettingsLog.Edit(log);
                }
            }
            get
            {
                string value;
                using (var repoUnit = new RepoUnit())
                {
                    value = repoUnit.SettingsLog.FindFirstBy(f => f.Name == "fansName").SValue;
                }
                return value;
            }
        }

        public RangeValueElement AirConsumption
        {
            set
            {
                using (var repoUnit = new RepoUnit())
                {
                    var log = repoUnit.SettingsLog.FindFirstBy(f => f.Name == "airConsumption");
                    log.Warning = value.WarningLevel;
                    log.Danger = value.DangerLevel;
                    repoUnit.SettingsLog.Edit(log);
                }
            }
            get
            {
                RangeValueElement value = new RangeValueElement();
                using (var repoUnit = new RepoUnit())
                {
                    var log = repoUnit.SettingsLog.FindFirstBy(f => f.Name == "airConsumption");
                    value.WarningLevel = log.Warning;
                    value.DangerLevel = log.Danger;
                }
                return value;
            }
        }

        public RangeValueElement Pressure
        {
            set
            {
                using (var repoUnit = new RepoUnit())
                {
                    var log = repoUnit.SettingsLog.FindFirstBy(f => f.Name == "pressure");
                    log.Warning = value.WarningLevel;
                    log.Danger = value.DangerLevel;
                    repoUnit.SettingsLog.Edit(log);
                }
            }
            get
            {
                RangeValueElement value = new RangeValueElement();
                using (var repoUnit = new RepoUnit())
                {
                    var log = repoUnit.SettingsLog.FindFirstBy(f => f.Name == "pressure");
                    value.WarningLevel = log.Warning;
                    value.DangerLevel = log.Danger;
                }
                return value;
            }
        }

        public RangeValueElement PillowTemperature
        {
            set
            {
                using (var repoUnit = new RepoUnit())
                {
                    var log = repoUnit.SettingsLog.FindFirstBy(f => f.Name == "pillowTemperature");
                    log.Warning = value.WarningLevel;
                    log.Danger = value.DangerLevel;
                    repoUnit.SettingsLog.Edit(log);
                }
            }
            get
            {
                RangeValueElement value = new RangeValueElement();
                using (var repoUnit = new RepoUnit())
                {
                    var log = repoUnit.SettingsLog.FindFirstBy(f => f.Name == "pillowTemperature");
                    value.WarningLevel = log.Warning;
                    value.DangerLevel = log.Danger;
                }
                return value;
            }
        }

        public RangeValueElement PillowVibration
        {
            set
            {
                using (var repoUnit = new RepoUnit())
                {
                    var log = repoUnit.SettingsLog.FindFirstBy(f => f.Name == "pillowVibration");
                    log.Warning = value.WarningLevel;
                    log.Danger = value.DangerLevel;
                    repoUnit.SettingsLog.Edit(log);
                }
            }
            get
            {
                RangeValueElement value = new RangeValueElement();
                using (var repoUnit = new RepoUnit())
                {
                    var log = repoUnit.SettingsLog.FindFirstBy(f => f.Name == "pillowVibration");
                    value.WarningLevel = log.Warning;
                    value.DangerLevel = log.Danger;
                }
                return value;
            }
        }
        public RangeValueElement GprsQuality
        {
            set
            {
                using (var repoUnit = new RepoUnit())
                {
                    var log = repoUnit.SettingsLog.FindFirstBy(f => f.Name == "gprsQuality");
                    log.Warning = value.WarningLevel;
                    log.Danger = value.DangerLevel;
                    repoUnit.SettingsLog.Edit(log);
                }
            }
            get
            {
                RangeValueElement value = new RangeValueElement();
                using (var repoUnit = new RepoUnit())
                {
                    var log = repoUnit.SettingsLog.FindFirstBy(f => f.Name == "gprsQuality");
                    value.WarningLevel = log.Warning;
                    value.DangerLevel = log.Danger;
                }
                return value;
            }
        }
    }
}
