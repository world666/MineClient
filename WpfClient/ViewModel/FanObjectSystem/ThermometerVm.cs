using System.Collections.Generic;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using Mc.Settings.Model.Concrete;
using WpfClient.Model.Entities;

namespace WpfClient.ViewModel.FanObjectSystem
{
    class ThermometerVm : ViewModelBase
    {
        private readonly int _thermometerCount = 4;
        private  double _maxTemperature;

        public ObservableCollection<double> Levels { get; set; }
        public double MaxTemperature
        {
            get { return _maxTemperature; }
            set { _maxTemperature = value; RaisePropertyChanged("MaxTemperature"); }
        }  

        public ThermometerVm()
        {
            _maxTemperature = Config.Instance.MaxTemperature;
            initialize();
        }

        public void Update(List<Parameter> temperatures)
        {
            for (int i = 0; i < temperatures.Count; i++)
            {
                Levels[i] = temperatures[i].Value / _maxTemperature;    
            }
            MaxTemperature = _maxTemperature;
        }

        private void initialize()
        {
            Levels = new ObservableCollection<double>();
            for (var i = 0; i < _thermometerCount; i++) Levels.Add(0);    
        }
    }
}
