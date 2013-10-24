using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Mc.Settings.Model.Concrete;
using WpfClient.Model;
using WpfClient.Model.Plot;
using WpfClient.Model.Settings;
using WpfClient.Services;

namespace WpfClient.ViewModel.Settings
{
    class FanObjectSettingsVm : ViewModelBase 
    {
        private int _fanObjectCount;
        public ObservableCollection<CheckBoxData> ListCollectionAnalog { get; set; }
        public FanObjectSettingsVm()
        {
            initialize();
        }

         ~FanObjectSettingsVm()
        {
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
            StatorCurrent = new RangeValueElementVm(Config.Instance.FanObjectConfig.StatorCurrent);
            RotorCurrentLow = new RangeValueElementVm(Config.Instance.FanObjectConfig.RotorCurrentLow);
            RotorCurrentHigh = new RangeValueElementVm(Config.Instance.FanObjectConfig.RotorCurrentHigh);
            OilFlow = new RangeValueElementVm(Config.Instance.FanObjectConfig.OilFlow);

            GprsQuality = new RangeValueElementVm(Config.Instance.FanObjectConfig.GprsQuality);
            FanNames = new ObservableCollection<FanObjectConfigData>();
            LoadFanNames();
            LoadDataInCheckList();
        }


        private RelayCommand _saveCommand;
        public ICommand SaveClick
        {
            get { return _saveCommand ?? (_saveCommand = new RelayCommand(SaveClickHandler)); }
        }

        private void SaveClickHandler()
        {
            string[] fanNames = FanNames.Select(f=>f.FanName).ToArray();
            List<string> checkList = new List<string>();
            for (int i = 0; i < ListCollectionAnalog.Count; i++)
            {
                if (ListCollectionAnalog[i].IsChecked)
                {
                    checkList.Add(i.ToString());
                }
            }
            Config.Instance.FanObjectConfig.FansName = fanNames;
            Config.Instance.FanObjectConfig.GeneralAnalogSignalsView = checkList.ToArray();
            Config.Instance.FanObjectConfig.PillowTemperature = PillowTemperature.RangeValueElement;
            Config.Instance.FanObjectConfig.PillowVibration = PillowVibration.RangeValueElement;
            Config.Instance.FanObjectConfig.Pressure = Pressure.RangeValueElement;
            Config.Instance.FanObjectConfig.AirConsumption = AirConsumption.RangeValueElement;
            Config.Instance.FanObjectConfig.StatorCurrent = StatorCurrent.RangeValueElement;
            Config.Instance.FanObjectConfig.RotorCurrentLow = RotorCurrentLow.RangeValueElement;
            Config.Instance.FanObjectConfig.RotorCurrentHigh = RotorCurrentHigh.RangeValueElement;
            Config.Instance.FanObjectConfig.OilFlow = OilFlow.RangeValueElement;
            Config.Instance.FanObjectConfig.GprsQuality = GprsQuality.RangeValueElement;
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

        public RangeValueElementVm PillowTemperature {get; set; }

        public RangeValueElementVm PillowVibration { get; set; }

        public RangeValueElementVm Pressure { get; set; }

        public RangeValueElementVm AirConsumption { get; set; }

        public RangeValueElementVm StatorCurrent { get; set; }

        public RangeValueElementVm RotorCurrentLow { get; set; }

        public RangeValueElementVm RotorCurrentHigh { get; set; }

        public RangeValueElementVm OilFlow { get; set; }

        public RangeValueElementVm GprsQuality { get; set; }

        private ObservableCollection<FanObjectConfigData> _fanNames;
        public ObservableCollection<FanObjectConfigData> FanNames
        {
            get { return _fanNames; }
            set { _fanNames = value; RaisePropertyChanged("FanNames"); }
        }

        private void LoadDataInCheckList()
        {
            ListCollectionAnalog = new ObservableCollection<CheckBoxData>();
            List<string> names = IoC.Resolve<DatabaseService>().GetAnalogSignalNames();
            names.ForEach(n => ListCollectionAnalog.Add(new CheckBoxData { IsChecked = false, Name = n }));
            int i = 0;
            string[] checkStr = Config.Instance.FanObjectConfig.GeneralAnalogSignalsView; 
            foreach (var checkBox in ListCollectionAnalog)
            {
                checkBox.ClickEvent += CheckBoxClickHandler;
                if (checkStr.Contains(i.ToString()))
                    checkBox.IsChecked = true;
                i++;
            }
        }
        private void LoadFanNames()
        {
            string[] fanNames = Config.Instance.FanObjectConfig.FansName;
            for (int i = 0; i < _fanObjectCount; i++)
            {
                FanNames.Add(i < fanNames.Count()
                                 ? new FanObjectConfigData {FanName = fanNames[i]}
                                 : new FanObjectConfigData {FanName = ""});
            }
        }

        public void CheckBoxClickHandler(string paramName, bool isChecked)
        {

        }
    }
}
