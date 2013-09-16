using System.Collections.Generic;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using WpfClient.Model.Concrete;
using WpfClient.Model.Entities;

namespace WpfClient.ViewModel.FanObjectSystem
{
    class ThermometerVm : ViewModelBase
    {
        private readonly int _thermometerCount = 4;
        private readonly double _maxTemperature;

        public ObservableCollection<double> Levels { get; set; }  

        public ThermometerVm()
        {
            _maxTemperature = Config.Instance.MaxTemperature;
            initialize();
        }

        public void Update(List<Parameter> temperatures)
        {
            for (var i = 0; i < _thermometerCount; i++)
            {
                Levels[i] = temperatures[0].Value / _maxTemperature;
            }
        }

        private void initialize()
        {
            Levels = new ObservableCollection<double>();
            for (var i = 0; i < _thermometerCount; i++) Levels.Add(0);
        }
    }
}
