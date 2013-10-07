using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using DataRepository.Models;
using GalaSoft.MvvmLight;
using Mc.Settings.Model.Concrete;
using WpfClient.Model;
using WpfClient.Model.Abstract;
using WpfClient.Model.Concrete;
using WpfClient.Model.Entities;
using WpfClient.Services;

namespace WpfClient.ViewModel.General
{
    public class GeneralVm : ViewModelBase, IDisposable, IUpDatable
    {
        private List<FanVm> _fans;
        private ObservableCollection<string> _signalNames;
        private Timer timer;
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
            timer = new Timer(10000);
            timer.Elapsed += (sender, args) => UpdateSignalState();
            timer.Start();
            timer.Enabled = true;
        }
        public void Dispose()
        {
            timer.Dispose();
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
                var signals = new List<string> {"Состояние сигнала","Вентилятор в работе", "Режим работы вентилятора"};
                signals.AddRange(_databaseService.GetAnalogSignalNames());

                return signals;
            }).ContinueWith(task => task.Result.ForEach(s => _signalNames.Add(s)));
        }
        public void UpdateSignalState()
        {
            foreach (var fan in _fans)
            {
                if(fan.Values.Count==0)
                    return;
                fan.Values[0].Value = _fanService.checkRemoteSignalState(fan.FanObjectId).Value;
                fan.Values[0].Maximum = _fanService.checkRemoteSignalState(fan.FanObjectId).Maximum;
                fan.Values[0].State = _fanService.checkRemoteSignalState(fan.FanObjectId).State;
            }
        }
        public void Update(FanLog fanLog)
        {
            if (_fans.Count != Config.Instance.FanObjectConfig.FanObjectCount)
                initialize();
            var parameterList = new List<ParameterVm>();
            var fanObject = _databaseService.GetFanObject(fanLog);
            if (fanObject == null) return;
            parameterList.Add(_fanService.checkRemoteSignalState(fanObject.FanObjectId));
            parameterList.Add(_fanService.getFanNumberParameter(fanObject));
            parameterList.Add(getFanStateParameter(fanObject));
            fanObject.Parameters.ForEach(p => parameterList.Add(new ParameterVm(p)));
            _fans[fanObject.FanObjectId-1].Values = parameterList;
        }
        private void updateFanValues()
        {
            if(_fans.Count!=Config.Instance.FanObjectConfig.FanObjectCount)
                initialize();
            var parametersList = new List<List<ParameterVm>>();

            for (var i = 1; i <= _fans.Count; i++)
            {
                var fanObject = _databaseService.GetFanObject(i);
                if (fanObject == null) continue;

                parametersList.Add(new List<ParameterVm>());

                parametersList[i - 1].Add(_fanService.checkRemoteSignalState(i));
                parametersList[i - 1].Add(_fanService.getFanNumberParameter(fanObject));
                parametersList[i - 1].Add(getFanStateParameter(fanObject));
                fanObject.Parameters.ForEach(p => parametersList[i - 1].Add(new ParameterVm(p)));
            }
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                for (var i = 0; i < parametersList.Count; i++) _fans[i].Values = parametersList[i];
            }));
        }

        

        private ParameterVm getFanStateParameter(FanObject fanObject)
        {
            return _fanService.GetFanMode(fanObject.WorkingFanNumber, fanObject.Doors);
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
