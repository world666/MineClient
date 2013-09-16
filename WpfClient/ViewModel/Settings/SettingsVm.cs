using System;
using System.Collections.Generic;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using WpfClient.Model;
using WpfClient.ViewModel.General;

namespace WpfClient.ViewModel.Settings
{
    class SettingsVm : ViewModelBase 
    {
        private object _currentView;
        private RelayCommand<object> _optionCommand;
        private RelayCommand _backArrowClickCommand;
        public ICommand BackArrowClick
        {
            get { return _backArrowClickCommand ?? (_backArrowClickCommand = new RelayCommand(BackArrowClickHandler)); }
        }
        private void BackArrowClickHandler()
        {
            IoC.Resolve<MainVm>().CurrentView = IoC.Resolve<GeneralVm>();
        }
        public SettingsVm()
        {        
        }

        public object CurrentView 
        {
            get { return _currentView; }
            set 
            {
                _currentView = value;
                RaisePropertyChanged("CurrentView");
            }
        }

        public List<string> SettingNames { get; set; }

        public ICommand MenuClick {
            get { return _optionCommand ?? (_optionCommand = new RelayCommand<object>(SelectedOptionControl)); }
        }

        private void SelectedOptionControl(object t) {
            var selectedOption = (string) t;

            if (selectedOption.Equals("FanObjectSettings", StringComparison.InvariantCulture))
                CurrentView = IoC.Resolve<FanObjectSettingsVm>();
            if (selectedOption.Equals("ColorSettings", StringComparison.InvariantCulture))
                CurrentView = IoC.Resolve<FanObjectSettingsVm>();
        }
    }
}
