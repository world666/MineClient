using System;
using System.Configuration;

namespace WpfClient.Model.Settings
{
    public class FanObjectConfigSection : ConfigurationSection 
    {
        [ConfigurationProperty("FanObjectCount", DefaultValue = "1", IsRequired = true)]
        public Int32 FanObjectCount
        {
            get { return (Int32)this["FanObjectCount"]; }
            set { this["FanObjectCount"] = value; }
        }

        [ConfigurationProperty("mineName")]
        public string MineName
        {
            get { return (string)this["mineName"]; }
            set { this["mineName"] = value; }
        }

        [ConfigurationProperty("fansName")]
        public string FansName
        {
            get { return (string)this["fansName"]; }
            set { this["fansName"] = value; }
        }

        [ConfigurationProperty("airConsumption")]
        public RangeValueElement AirConsumption
        {
            get { return (RangeValueElement)this["airConsumption"]; }
            set
            {
                this["airConsumption"] = value;
            }
        }

        [ConfigurationProperty("pressure")]
        public RangeValueElement Pressure
        {
            get { return (RangeValueElement)this["pressure"]; }
            set { this["pressure"] = value; }
        }

        [ConfigurationProperty("pillowTemperature")]
        public RangeValueElement PillowTemperature
        {
            get { return (RangeValueElement)this["pillowTemperature"]; }
            set { this["pillowTemperature"] = value; }
        }

        [ConfigurationProperty("pillowVibration")]
        public RangeValueElement PillowVibration 
        {
            get { return (RangeValueElement)this["pillowVibration"]; }
            set { this["pillowVibration"] = value; }
        }
        [ConfigurationProperty("gprsQuality")]
        public RangeValueElement GprsQuality
        {
            get { return (RangeValueElement)this["gprsQuality"]; }
            set { this["gprsQuality"] = value; }
        }
    }
}
