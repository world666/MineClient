namespace WpfClient.Model.Settings
{
    public class RangeValueElementVm
    {
        private double _warningLevel;
        private double _dangerLevel;
        private readonly RangeValueElement _rangeValueElement;

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


        public RangeValueElementVm(RangeValueElement rangeValueElement)
        {
            _warningLevel = rangeValueElement.WarningLevel;
            _dangerLevel = rangeValueElement.DangerLevel;
            _rangeValueElement = rangeValueElement;
        }

        
    }
}
