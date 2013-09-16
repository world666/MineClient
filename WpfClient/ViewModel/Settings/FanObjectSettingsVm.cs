using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using WpfClient.Model.Concrete;
using WpfClient.Model.Settings;

namespace WpfClient.ViewModel.Settings
{
    class FanObjectSettingsVm : ViewModelBase 
    {
        private int _fanObjectCount;

        public FanObjectSettingsVm()
        {
            initialize();
        }

         ~FanObjectSettingsVm()
        {
            Config.Instance.Save();
        }

        private void initialize()
        {
            _fanObjectCount = Config.Instance.FanObjectConfig.FanObjectCount;
            MineName = Config.Instance.FanObjectConfig.MineName;
            FanObjectCounts = new int[] { 1, 2, 3, 4, 5, 6 };

            PillowTemperature = new RangeValueElementVm(Config.Instance.FanObjectConfig.PillowTemperature);
            PillowVibration = new RangeValueElementVm(Config.Instance.FanObjectConfig.PillowVibration);
            Pressure = new RangeValueElementVm(Config.Instance.FanObjectConfig.Pressure);
            AirConsumption = new RangeValueElementVm(Config.Instance.FanObjectConfig.AirConsumption);
            GprsQuality = new RangeValueElementVm(Config.Instance.FanObjectConfig.GprsQuality);
            FanNames = new ObservableCollection<FanObjectConfigData>();
            string[] fanNames = Config.Instance.FanObjectConfig.FansName.Split(new string[] { "!$!" },StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < _fanObjectCount; i++)
            {
                if (i < fanNames.Count())
                    FanNames.Add(new FanObjectConfigData { FanName = fanNames[i] });
                else
                    FanNames.Add(new FanObjectConfigData { FanName = "" });
            }
        }


        private RelayCommand _saveCommand;
        public ICommand SaveClick
        {
            get { return _saveCommand ?? (_saveCommand = new RelayCommand(SaveClickHandler)); }
        }

        private void SaveClickHandler()
        {
            string fanNames = FanNames.Aggregate("", (current, fanName) => current + fanName.FanName+"!$!");
            Config.Instance.FanObjectConfig.FansName = fanNames;
        }

        public int FanObjectCount
        {
            get { return _fanObjectCount; }
            set
            {
                _fanObjectCount = value;
                Config.Instance.FanObjectConfig.FanObjectCount = value;
                RaisePropertyChanged("FanObjectCount");
                initialize();
            }
        }

        public int[] FanObjectCounts { get; set; }

        private string _mineName;
        public string MineName 
        {
            get { return _mineName; }
            set { _mineName = value;
                Config.Instance.FanObjectConfig.MineName = value;
                RaisePropertyChanged("MineName");
            }
        }

        public RangeValueElementVm PillowTemperature { get; set; }

        public RangeValueElementVm PillowVibration { get; set; }

        public RangeValueElementVm Pressure { get; set; }

        public RangeValueElementVm AirConsumption { get; set; }

        public RangeValueElementVm GprsQuality { get; set; }

        private ObservableCollection<FanObjectConfigData> _fanNames;
        public ObservableCollection<FanObjectConfigData> FanNames
        {
            get { return _fanNames; }
            set { _fanNames = value; RaisePropertyChanged("FanNames"); }
        }
    }
}
