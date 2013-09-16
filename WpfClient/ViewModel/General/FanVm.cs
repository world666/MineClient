using System;
using System.Collections.Generic;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Ninject.Parameters;
using WpfClient.Model;
using WpfClient.Model.Concrete;
using WpfClient.ViewModel.FanObjectSystem;
using WpfClient.ViewModel.Plot;

namespace WpfClient.ViewModel.General
{
    public class FanVm : ViewModelBase
    {
        private List<ParameterVm> _parameters;

        private RelayCommand<object> _paramClickCommand;
        private RelayCommand<object> _fanClickCommand;

        public FanVm(int fanObjectId)
        {
            FanObjectId = fanObjectId;
        }

        public int FanObjectId { get; private set; }

        public string FanName
        {
            get {
                try
                {
                    return string.Format("Вентиляторная установка {0}",
                                         Config.Instance.FanObjectConfig.FansName.Split(new string[] {"!$!"},
                                                                                        StringSplitOptions
                                                                                            .RemoveEmptyEntries)[
                                                                                                FanObjectId - 1]);
                }
                catch (Exception)
                {
                    return string.Format("Вентиляторная установка №{0}", FanObjectId);
                }
            }
        }

        public List<ParameterVm> Values
        {
            get { return _parameters ?? (_parameters = new List<ParameterVm>()); }
            set
            {
                if (value != null) _parameters = value;
                RaisePropertyChanged("Values");
            }
        }

        public ICommand ParameterClick 
        {
            get { return _paramClickCommand ?? (_paramClickCommand = new RelayCommand<object>(OnParamClick)); }
        }

        public ICommand FanObjectClick 
        {
            get { return _fanClickCommand ?? (_fanClickCommand = new RelayCommand<object>(OnFanObjetClick)); }
        }

        public void OnFanObjetClick(object t)
        {
            IoC.Resolve<MainVm>().CurrentView = IoC.Resolve<FanObjectVm>(new ConstructorArgument("fanObjectId", FanObjectId));
        }

        private void OnParamClick(object t)
        {
            if ((int)t <= 1) return;

            var analogParametersVm = IoC.Resolve<PlotVm>(new ConstructorArgument("fanObjectId", FanObjectId), new ConstructorArgument("parameterNum", (int)t - 2));
            IoC.Resolve<MainVm>().CurrentView = analogParametersVm;
        }
    }
}
