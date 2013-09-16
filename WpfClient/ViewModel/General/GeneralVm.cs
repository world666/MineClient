using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight;
using WpfClient.Model;
using WpfClient.Model.Concrete;
using WpfClient.Model.Entities;
using WpfClient.Services;

namespace WpfClient.ViewModel.General
{
    public class GeneralVm : ViewModelBase
    {
        private List<FanVm> _fans;
        private ObservableCollection<string> _signalNames;

        private readonly DatabaseService _databaseService;
        private readonly FanService _fanService;
        private ParameterVm _remoteSignalState { get; set; }

        public GeneralVm(DatabaseService databaseService, FanService fanService)
        {
            _databaseService = databaseService;
            _fanService = fanService;

            initialize();

            updateFanValues();
            setParameterNames();

            AsyncProvider.StartTimer(10000, updateFanValues);
        }

        public DateTimeVm DateTime { get { return IoC.Resolve<DateTimeVm>(); } }

        public ObservableCollection<string> Signals 
        {
            get
            {
                if (_signalNames == null) setParameterNames();
                return _signalNames;
            }
        }

        public List<FanVm> Fans { get { return _fans; } }

        public ParameterVm RemoteSignalState
        {
            get { return _remoteSignalState; }
            set
            {
                _remoteSignalState = value;
                RaisePropertyChanged("RemoteSignalState");
            }
        }

        public string MineName
        {
            get { return Config.Instance.FanObjectConfig.MineName; }
        }

        private void setParameterNames()
        {
            _signalNames = new ObservableCollection<string>();

            Task.Run(() =>
            {
                var signals = new List<string> {"Состояние сигнала","Вентилятор в работе", "Состояние вентилятора"};
                signals.AddRange(_databaseService.GetAnalogSignalNames());

                return signals;
            }).ContinueWith(task => task.Result.ForEach(s => _signalNames.Add(s)));
        }

        private void updateFanValues()
        {
            var parametersList = new List<List<ParameterVm>>();

            for (var i = 1; i <= _fans.Count; i++)
            {
                parametersList.Add(new List<ParameterVm>());

                var fanObject = _databaseService.GetFanObject(i);
                if (fanObject == null) continue;

                parametersList[i - 1].Add(checkRemoteSignalState(i));
                parametersList[i - 1].Add(getFanNumberParameter(fanObject));
                parametersList[i - 1].Add(getFanStateParameter(fanObject));
                fanObject.Parameters.ForEach(p => parametersList[i - 1].Add(new ParameterVm(p)));
            }
            IoC.Resolve<HTTPService>().WriteDataToIndexFile(parametersList);//save data to index.html for http server
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                for (var i = 0; i < parametersList.Count; i++) _fans[i].Values = parametersList[i];
            }));
        }

        private ParameterVm checkRemoteSignalState(int fanObjectId)
        {
            ParameterVm SignalState = new ParameterVm();
            SignalState.Name = "Состояние сигнала";
            if (System.DateTime.Now - RemoteService.GetLastRecieve(fanObjectId) > new TimeSpan(0, 1, 0))
            {
                SignalState.Value = "отсутствует";
                SignalState.State = StateEnum.Dangerous;
            }
            else
            {
                SignalState.Value = "стабильный";
                SignalState.State = StateEnum.Ok;
            }
            return SignalState;
        }

        private ParameterVm getFanStateParameter(FanObject fanObject)
        {
            return _fanService.GetFanMode(fanObject.WorkingFanNumber, fanObject.Doors);
        }

        private ParameterVm getFanNumberParameter(FanObject fanObject)
        {
            var parameter = new ParameterVm {Value = fanObject.WorkingFanNumber == 0 ? "АВАРИЯ" : string.Format("№{0}", fanObject.WorkingFanNumber), Name = "Вентилятор в работе"};
            parameter.State = SystemStateService.GetFanState(fanObject.WorkingFanNumber);

            return parameter;
        }

        private void initialize()
        {
            //RemoteSignalState = new ParameterVm { Name = "Состояние сигнала:", Value = "неопределено" };
            _fans = new List<FanVm>();
            for (int i = 1; i <= Config.Instance.FanObjectConfig.FanObjectCount; i++) 
            {
                _fans.Add(new FanVm(i));
            }
        }
    }
}
