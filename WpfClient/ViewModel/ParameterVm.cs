using System.Globalization;
using GalaSoft.MvvmLight;
using WpfClient.Model;
using WpfClient.Model.Entities;

namespace WpfClient.ViewModel
{
    public class ParameterVm : ViewModelBase
    {        
        public ParameterVm(Parameter parameter)
        {
            Value = parameter.Value.ToString(CultureInfo.InvariantCulture);
            Name = parameter.Name;
            State = parameter.State;
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
        public string Name { get; set; }
        private StateEnum _state;
        public StateEnum State
        {
            get { return _state; }
            set { _state = value; RaisePropertyChanged("State"); }
        }
    }
}
