using Mc.Settings.Model.Settings;

namespace WpfClient.Model.Settings
{
    public class RangeValueElementVm
    {
        private double _warningLevel;
        private double _dangerLevel;
        private RangeValueElement _rangeValueElement;

        public double DangerLevel
        {
            get { return _dangerLevel; }
            set
            {
                _dangerLevel = value;
                _rangeValueElement.DangerLevel = value;
            }
        }

        public double WarningLevel
        {
            get { return _warningLevel; }
            set
            {
                _warningLevel = value;
                _rangeValueElement.WarningLevel = value;
            }
        }

        public RangeValueElement RangeValueElement {
            get { return _rangeValueElement; }
        }

        public RangeValueElementVm(RangeValueElement rangeValueElement)
        {
            _warningLevel = rangeValueElement.WarningLevel;
            _dangerLevel = rangeValueElement.DangerLevel;
            _rangeValueElement = rangeValueElement;
        }

        
    }
}
