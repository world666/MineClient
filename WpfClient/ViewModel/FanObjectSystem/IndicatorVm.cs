using System.Collections.Generic;
using System.Collections.ObjectModel;
using Mc.Settings.Model.Concrete;
using WpfClient.Model.Concrete;
using WpfClient.Model.Entities;

namespace WpfClient.ViewModel.FanObjectSystem
{
    class IndicatorVm
    {
        private readonly int _indicatorCount = 6;
        private readonly double _maxAirFlowValue;
        private readonly double _maxPressureValue;
        private readonly double _maxPillowValue;

        public ObservableCollection<double> Levels { get; set; }
        public ObservableCollection<double> Values { get; set; }

        public IndicatorVm()
        {
            _maxAirFlowValue = Config.Instance.MaxAirFlowValue;
            _maxPressureValue = Config.Instance.MaxPressureValue;
            _maxPillowValue = Config.Instance.MaxPillowValue;

            initialize();
        }

        public void Update(List<Parameter> indicatorValues)
        {
            Values[0] = indicatorValues[0].Value;
            Values[1] = indicatorValues[1].Value;
            Levels[0] = Values[0] / _maxAirFlowValue;
            Levels[1] = Values[1] / _maxPressureValue;

            for (var i = 2; i < indicatorValues.Count; i++)
            {
                Values[i] = indicatorValues[i].Value;
                Levels[i] = indicatorValues[i].Value / _maxPillowValue;
            }
        }

        private void initialize()
        {
            Levels = new ObservableCollection<double>();
            Values = new ObservableCollection<double>();
            
            for (var i = 0; i < _indicatorCount; i++)
            {
                Levels.Add(0);
                Values.Add(0);
            }
        }
    }
}
