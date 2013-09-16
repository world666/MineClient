using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace WpfClient.Model.Plot
{
    public class PlotData : ViewModelBase
    {

        public string Name { get; set; }
        private bool _isChecked;
        public bool IsChecked 
        {
            get { return _isChecked; }
            set { _isChecked = value; RaisePropertyChanged("IsChecked"); }
        }

        public delegate void CheckBoxClickHandler(string paramName, bool isChacked);

        public event CheckBoxClickHandler ClickEvent;

        private RelayCommand _clickCommand;
        public ICommand Click
        {
            get { return _clickCommand ?? (_clickCommand = new RelayCommand(ClickHandler)); }
        }
        private void ClickHandler()
        {
            if (ClickEvent != null)
                ClickEvent(Name, IsChecked);
        }
    }
}
