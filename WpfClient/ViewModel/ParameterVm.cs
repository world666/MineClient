using System.Globalization;
using GalaSoft.MvvmLight;
using WpfClient.Model;
using WpfClient.Model.Entities;
using WpfClient.Services;

namespace WpfClient.ViewModel
{
    public class ParameterVm : ViewModelBase
    {        
        public ParameterVm(Parameter parameter)
        {
            Value = parameter.Value.ToString(CultureInfo.InvariantCulture);
            Name = parameter.Name;
            State = parameter.State;
            Maximum = SystemStateService.GetMaximumValue(Name).ToString();
        }

        public ParameterVm()
        {
        }

        private string _value;
        public string Value
        {
            get { return _value; } 
            set { _value = value; RaisePropertyChanged("Value"); }
        }

        private string _maximum;
        public string Maximum
        {
            get { return _maximum; }
            set { _maximum = value; RaisePropertyChanged("Maximum"); }
        }
        public string Name { get; set; }
        private StateEnum _state;
        public StateEnum State
        {
            get { return _state; }
            set { _state = value; RaisePropertyChanged("State"); }
        }
    }
}
