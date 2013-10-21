using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DataRepository.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Mc.CustomControls.Model;
using WpfClient.Model;
using WpfClient.Model.Abstract;
using WpfClient.Model.Entities;
using WpfClient.Services;
using WpfClient.ViewModel.FanObjectSystem;
using WpfClient.ViewModel.General;

namespace WpfClient.ViewModel
{
    class ElectricDriveVm : ViewModelBase,IDisposable,IUpDatable 
    {
        
        public ObservableCollection<double> Temperatures { get; set; }
        public ObservableCollection<double> Pillovs { get; set; }
        public ObservableCollection<StateEnum> TemperaturesState { get; set; }
        public ObservableCollection<StateEnum> PillovsState { get; set; }
        private double _naAngleTop;
        public double NaAngleTop {
            get { return _naAngleTop; }
            set { _naAngleTop = value; RaisePropertyChanged("NaAngleTop"); }
        }
        private double _naAngleBottom;
        public double NaAngleBottom
        {
            get { return _naAngleBottom; }
            set { _naAngleBottom = value; RaisePropertyChanged("NaAngleBottom"); }
        }
        private string _naState;
        public string NaState
        {
            get { return _naState; }
            set { _naState = value; RaisePropertyChanged("NaState"); }
        }
        private StateEnum _naStateColor;
        public StateEnum NaStateColor
        {
            get { return _naStateColor; }
            set { _naStateColor = value; RaisePropertyChanged("NaStateColor"); }
        }
        private RelayCommand _backArrowClickCommand;
        public ICommand BackArrowClick
        {
            get { return _backArrowClickCommand ?? (_backArrowClickCommand = new RelayCommand(BackArrowClickHandler)); }
        }
        
        private void BackArrowClickHandler()
        {
            IDisposable dispose = (IDisposable)IoC.Resolve<MainVm>().CurrentView;
            dispose.Dispose();
            IoC.Resolve<MainVm>().CurrentView = _prevView;
        }
        public ElectricDriveVm( FanObjectVm prevView, int fanObjectId=1, int fanId=1)
        {
            _fanObjectId = fanObjectId;
            _fanId = fanId;
            _prevView = prevView;
            initialize();
            LoadFromDb();
        }
        public void Dispose()
        {   
        }
        public void LoadFromDb()
        {
            var fanObject = IoC.Resolve<DatabaseService>().GetFanObject(_fanObjectId);
            Update(fanObject);
        }
        public void Update(FanLog fanLog)
        {
            var fanObject = IoC.Resolve<DatabaseService>().GetFanObject(fanLog);
            Update(fanObject);
        }
        private void Update(FanObject fanObject)
        {
            if (fanObject == null)
                return;
            for (int i = 2; i <= 5; i++)
            {
                Temperatures[i - 2] = fanObject.Parameters[i].Value;
                TemperaturesState[i - 2] = fanObject.Parameters[i].State;
            }
            for (int i = 6; i <= 9; i++)
            {
                Pillovs[i - 6] = fanObject.Parameters[i].Value;
                PillovsState[i - 6] = fanObject.Parameters[i].State;
            }
            int stateId;
            if(_fanId==1)
                stateId = fanObject.Doors[(int)WpfClient.Model.DoorType.mb114 -1].StateId;//направляющий апарат 1/2
            else//=2
                stateId = fanObject.Doors[(int)WpfClient.Model.DoorType.mb115 -1].StateId;//направляющий апарат 1/2
            var stateEnum = Enum.IsDefined(typeof(DoorStateEnum), stateId) ? (DoorStateEnum)stateId : DoorStateEnum.Undefined;
            NaState = stateEnum == DoorStateEnum.Open
                          ? "Открыт"
                          : (stateEnum == DoorStateEnum.Close ? "Закрыт" : "Неопределен");
            NaStateColor = stateEnum == DoorStateEnum.Open
                               ? StateEnum.Ok
                               : (stateEnum == DoorStateEnum.Close ? StateEnum.Warning : StateEnum.Dangerous);
            NaAngleTop = stateEnum == DoorStateEnum.Open
                          ? -160
                          : (stateEnum == DoorStateEnum.Close ? -180 : -90);
            NaAngleBottom = stateEnum == DoorStateEnum.Open
                          ? -20
                          : (stateEnum == DoorStateEnum.Close ? 0 : -90);
        }
        private void initialize()
        {
            Temperatures = new ObservableCollection<double>();
            Pillovs = new ObservableCollection<double>();
            TemperaturesState = new ObservableCollection<StateEnum>();
            PillovsState = new ObservableCollection<StateEnum>();
            for (var i = 0; i < _thermometerCount; i++)
            {
                Temperatures.Add(0);
                TemperaturesState.Add(0);
            }
            for (var i = 0; i < _indicatorCount; i++)
            {
                Pillovs.Add(0);
                PillovsState.Add(0);
            }
        }
        private readonly int _thermometerCount = 4;
        private readonly int _indicatorCount = 4;
        private int _fanObjectId;
        private int _fanId;
        private FanObjectVm _prevView;
    }
}
