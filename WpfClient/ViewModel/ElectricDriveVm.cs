using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataRepository.Models;
using GalaSoft.MvvmLight;
using Mc.CustomControls.Model;
using WpfClient.Model;
using WpfClient.Model.Abstract;
using WpfClient.Services;

namespace WpfClient.ViewModel
{
    class ElectricDriveVm : ViewModelBase,IDisposable,IUpDatable 
    {
        private readonly int _thermometerCount = 4;
        private readonly int _indicatorCount = 4;
        public ObservableCollection<double> Temperatures { get; set; }
        public ObservableCollection<double> Pillovs { get; set; }
        private string _naState;
        public string NaState
        {
            get { return _naState; }
            set { _naState = value; RaisePropertyChanged("NaState"); }
        }
        private int _fanObjectId;
        private int _fanId;
        public ElectricDriveVm(int fanObjectId=1, int fanId=1)
        {
            _fanObjectId = fanObjectId;
            _fanId = fanId;
            initialize();
            LoadFromDb();
        }
        public void Dispose()
        {   
        }
        public void LoadFromDb()
        {
            var fanObject = IoC.Resolve<DatabaseService>().GetFanObject(_fanObjectId);
            if (fanObject == null)
                return;
            for (int i = 2; i <= 5; i++)
                Temperatures[i - 2] = fanObject.Parameters[i].Value;
            for (int i = 8; i <= 9; i++)
                Pillovs[i - 6] = fanObject.Parameters[i].Value;
            var stateId = fanObject.Doors[_fanId-1].StateId;//направляющий апарат 1/2
            var stateEnum = Enum.IsDefined(typeof(DoorStateEnum), stateId) ? (DoorStateEnum)stateId : DoorStateEnum.Undefined;
            NaState = stateEnum == DoorStateEnum.Open
                          ? "Открыт"
                          : (stateEnum == DoorStateEnum.Close ? "Закрыт" : "Неопределен");
        }
        public void Update(FanLog fanLog)
        {
            var fanObject = IoC.Resolve<DatabaseService>().GetFanObject(fanLog);
            if (fanObject == null)
                return;
            for (int i = 2; i <= 5; i++)
                Temperatures[i - 2] = fanObject.Parameters[i].Value;
            for (int i = 8; i <= 9; i++)
                Pillovs[i - 6] = fanObject.Parameters[i].Value;
            var stateId = fanObject.Doors[_fanId - 1].StateId;//направляющий апарат 1/2
            var stateEnum = Enum.IsDefined(typeof(DoorStateEnum), stateId) ? (DoorStateEnum)stateId : DoorStateEnum.Undefined;
            NaState = stateEnum == DoorStateEnum.Open
                          ? "Открыт"
                          : (stateEnum == DoorStateEnum.Close ? "Закрыт" : "Неопределен");
        }
        private void initialize()
        {
            Temperatures = new ObservableCollection<double>();
            Pillovs = new ObservableCollection<double>();
            for (var i = 0; i < _thermometerCount; i++) Temperatures.Add(0);
            for (var i = 0; i < _indicatorCount; i++) Pillovs.Add(0);
        }
    }
}
