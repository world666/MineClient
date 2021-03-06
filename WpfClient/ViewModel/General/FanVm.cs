using System;
using System.Collections.Generic;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Mc.Settings.Model.Concrete;
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
                                         Config.Instance.FanObjectConfig.FansName[FanObjectId - 1]);
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
            IDisposable dispose = (IDisposable)IoC.Resolve<MainVm>().CurrentView;
            dispose.Dispose();
            IoC.Resolve<MainVm>().CurrentView = IoC.Resolve<FanObjectVm>(new ConstructorArgument("fanObjectId", FanObjectId));
        }

        private void OnParamClick(object t)
        {
            if ((int)t <= 2) return;
            int realIndex = int.Parse(Config.Instance.FanObjectConfig.GeneralAnalogSignalsView[(int)t - 2 - 1]);
            var analogParametersVm = IoC.Resolve<PlotVm>(new ConstructorArgument("fanObjectId", FanObjectId), new ConstructorArgument("parameterNum", realIndex+1));
            IDisposable dispose = (IDisposable)IoC.Resolve<MainVm>().CurrentView;
            dispose.Dispose();
            IoC.Resolve<MainVm>().CurrentView = analogParametersVm;
        }
    }
}
