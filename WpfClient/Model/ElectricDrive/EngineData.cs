using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using Ninject.Parameters;
using WpfClient.ViewModel;

namespace WpfClient.Model.ElectricDrive
{
    public class EngineData:ViewModelBase
    {
        private ParameterVm _engineSpeed;

        public ParameterVm EngineSpeed
        {
            get { return _engineSpeed; }
            set { _engineSpeed = value; RaisePropertyChanged("EngineSpeed"); }
        }

        private ParameterVm _statorCurrent;

        public ParameterVm StatorCurent
        {
            get { return _statorCurrent; }
            set { _statorCurrent = value; RaisePropertyChanged("StatorCurent"); }
        }

        private ParameterVm _rotorCurrent;

        public ParameterVm RotorCurent
        {
            get { return _rotorCurrent; }
            set { _rotorCurrent = value; RaisePropertyChanged("RotorCurent"); }
        }
    }
}
