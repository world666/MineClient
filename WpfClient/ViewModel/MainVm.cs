using System;
using System.Data.Entity;
using System.Windows.Input;
using DataRepository.DataAccess;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Mc.HTTPServer;
using Ninject.Parameters;
using WpfClient.Model;
using WpfClient.Model.Abstract;
using WpfClient.Model.Concrete;
using WpfClient.Services;
using WpfClient.ViewModel.General;
using WpfClient.ViewModel.Plot;
using WpfClient.ViewModel.Settings;

namespace WpfClient.ViewModel
{
    public class MainVm : ViewModelBase
    {
        private object _currentView;
        private object _prevView;
        private WebServer _webServer;
        
        public MainVm()
        {
            Database.SetInitializer(new MineDbInitializer());
            
            IoC.Resolve<IRemoteListener>().InitServer("15000");//start and init tcp server for remote exchange
            //_webServer = new WebServer(90,RemouteFanControlService.SetData);//start web server port 90
            //HTTPService.HTTPServiceInit();//start write data to index file for web server
            CurrentView = IoC.Resolve<GeneralVm>();
        }

        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                PrevView = _currentView;
                _currentView = value;
                RaisePropertyChanged("CurrentView");
            }
        }

        public object PrevView
        {
            get { return _prevView; }
            private set
            {
                _prevView = value; 
            }
        }

        public string Str { get; set; }

        private RelayCommand<object> _viewMenuCommand;
        public ICommand MenuClick {
            get { return _viewMenuCommand ?? (_viewMenuCommand = new RelayCommand<object>(MenuViewControl)); }
        }

        private void MenuViewControl(object t) 
        {
            IDisposable dispose = (IDisposable)CurrentView;
            dispose.Dispose();

            var menuStr = t as string;

            if (menuStr.Equals("GeneralView", StringComparison.InvariantCulture))
                CurrentView = IoC.Resolve<GeneralVm>();
            if (menuStr.Equals("History", StringComparison.InvariantCulture))
                CurrentView = IoC.Resolve<HistoryVm>();
            if (menuStr.Equals("SettingsView", StringComparison.InvariantCulture))
                CurrentView = IoC.Resolve<SettingsVm>();
            if (menuStr.Equals("PlotView", StringComparison.InvariantCulture))
                CurrentView = IoC.Resolve<PlotVm>();
            if (menuStr.Equals("LogView", StringComparison.InvariantCulture))
                CurrentView = IoC.Resolve<LogVm>();
        }
    }
}