using System.Configuration;

namespace WpfClient.Model.Settings
{
    public class RangeValueElement : ConfigurationElement 
    {
        [ConfigurationProperty("warningLevel", DefaultValue = 0d,IsRequired = true)]
        public double WarningLevel
        {
            get { return (double) this["warningLevel"]; }
            set { this["warningLevel"] = value; }
        }

        [ConfigurationProperty("dangerLevel", DefaultValue = 0d, IsRequired = true)]
        public double DangerLevel
        {
            get { return (double)this["dangerLevel"]; }
            set { this["dangerLevel"] = value; }
        }
    }
}
