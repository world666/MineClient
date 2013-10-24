using System;
using System.Configuration;
using System.Linq;
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

        public string[] FansName
        {
            set
            {
                using (var repoUnit = new RepoUnit())
                {
                    var log = repoUnit.SettingsLog.FindFirstBy(f => f.Name == "fansName");
                    string fanNames = value.Aggregate("", (current, fanName) => current + fanName + "!$!");
                    log.SValue = fanNames;
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
                string[] result = value.Split(new string[] { "!$!" }, StringSplitOptions.RemoveEmptyEntries);
                return result;
            }
        }

        public string[] GeneralAnalogSignalsView
        {
            set
            {
                using (var repoUnit = new RepoUnit())
                {
                    var log = repoUnit.SettingsLog.FindFirstBy(f => f.Name == "generalAnalogSignalsView");
                    string generalAnalogSignalsView = value.Aggregate("", (current, checkElement) => current + checkElement + "!$!");
                    log.SValue = generalAnalogSignalsView;
                    repoUnit.SettingsLog.Edit(log);
                }
            }
            get
            {
                string value;
                using (var repoUnit = new RepoUnit())
                {
                    value = repoUnit.SettingsLog.FindFirstBy(f => f.Name == "generalAnalogSignalsView").SValue;
                }
                string[] result = value.Split(new string[] { "!$!" }, StringSplitOptions.RemoveEmptyEntries);
                return result;
            }
        }

        public RangeValueElement AirConsumption
        {
            set
            {
                WriteInDB("airConsumption", value);
            }
            get
            {
                return ReadFromDB("airConsumption");
            }
        }

        public RangeValueElement Pressure
        {
            set
            {
                WriteInDB("pressure", value);
            }
            get
            {
                return ReadFromDB("pressure");
            }
        }

        public RangeValueElement PillowTemperature
        {
            set
            {
                WriteInDB("pillowTemperature", value);
            }
            get
            {
                return ReadFromDB("pillowTemperature");
            }
        }

        public RangeValueElement PillowVibration
        {
            set
            {
                WriteInDB("pillowVibration", value);
            }
            get
            {
                return ReadFromDB("pillowVibration");
            }
        }
        public RangeValueElement StatorCurrent
        {
            set
            {
                WriteInDB("statorCurrent", value);
            }
            get
            {
                return ReadFromDB("statorCurrent");
            }
        }
        public RangeValueElement RotorCurrentLow
        {
            set
            {
                WriteInDB("rotorCurrentLow", value);
            }
            get
            {
                return ReadFromDB("rotorCurrentLow");
            }
        }
        public RangeValueElement RotorCurrentHigh
        {
            set
            {
                WriteInDB("rotorCurrentHigh", value);
            }
            get
            {
                return ReadFromDB("rotorCurrentHigh");
            }
        }
        public RangeValueElement OilFlow
        {
            set
            {
                WriteInDB("oilFlow", value);
            }
            get
            {
                return ReadFromDB("oilFlow");
            }
        }

        public RangeValueElement GprsQuality
        {
            set
            {
                WriteInDB("gprsQuality", value);
            }
            get
            {
                return ReadFromDB("gprsQuality");
            }
        }

        private void WriteInDB(string name,RangeValueElement rangeValueElement)
        {
            using (var repoUnit = new RepoUnit())
            {
                var log = repoUnit.SettingsLog.FindFirstBy(f => f.Name == name);
                log.Warning = rangeValueElement.WarningLevel;
                log.Danger = rangeValueElement.DangerLevel;
                repoUnit.SettingsLog.Edit(log);
            }
        }

        private RangeValueElement ReadFromDB(string name)
        {
            RangeValueElement value = new RangeValueElement();
            using (var repoUnit = new RepoUnit())
            {
                var log = repoUnit.SettingsLog.FindFirstBy(f => f.Name == name);
                value.WarningLevel = log.Warning;
                value.DangerLevel = log.Danger;
            }
            return value;
        }
    }
}
