using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using WpfClient.ViewModel;

namespace WpfClient.Model.ElectricDrive
{
    public class OilPumpData : ViewModelBase
    {
        private ParameterVm _oilTeperature;

        public ParameterVm OilTemperature
        {
            get { return _oilTeperature; }
            set { _oilTeperature = value; RaisePropertyChanged("OilTemperature"); }
        }

        private ParameterVm _oilPressure;

        public ParameterVm OilPressure
        {
            get { return _oilPressure; }
            set { _oilPressure = value; RaisePropertyChanged("OilPressure"); }
        }

        private ParameterVm _oilFlow;

        public ParameterVm OilFlow
        {
            get { return _oilFlow; }
            set { _oilFlow = value; RaisePropertyChanged("OilFlow"); }
        }
    }
}
