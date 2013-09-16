using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Ninject.Parameters;
using OxyPlot;
using WpfClient.Model;
using WpfClient.Model.Plot;
using WpfClient.Services;
using WpfClient.ViewModel.General;

namespace WpfClient.ViewModel.Plot
{
    class PlotVm : ViewModelBase
    {
        #region Property
        public ObservableCollection<PlotData> ListCollectionAnalog { get; set; }
        public ObservableCollection<PlotData> ListCollectionDigit { get; set; }
        private int _fanObjectId;
        public int FanObjectId
        {
            get { return _fanObjectId; }
            set { _fanObjectId = value; RaisePropertyChanged("FanObjectId"); }
        }
        private PlotModel _plotModelAnalog;
        public PlotModel PlotModelAnalog
        {
            get { return _plotModelAnalog; }
            set { _plotModelAnalog = value; RaisePropertyChanged("PlotModelAnalog"); }
        }

        private PlotModel _plotModelDigit;
        public PlotModel PlotModelDigit
        {
            get { return _plotModelDigit; }
            set { _plotModelDigit = value; RaisePropertyChanged("PlotModelDigit"); }
        }

        private string _dateDisplayFrom;
        public string DateDisplayFrom
        {
            get { return _dateDisplayFrom; }
            set { _dateDisplayFrom = value; RaisePropertyChanged("DateDisplayFrom"); }
        }

        private string _dateDisplayTo;
        public string DateDisplayTo
        {
            get { return _dateDisplayTo; }
            set { _dateDisplayTo = value; RaisePropertyChanged("DateDisplayTo"); }
        }

        private DateTime _dateSelectedFrom;
        public DateTime DateSelectedFrom
        {
            get { return _dateSelectedFrom; }
            set { _dateSelectedFrom = value; RaisePropertyChanged("DateSelectedFrom"); }
        }

        private DateTime _dateSelectedTo;
        public DateTime DateSelectedTo
        {
            get { return _dateSelectedTo; }
            set { _dateSelectedTo = value; RaisePropertyChanged("DateSelectedTo"); }
        }

        private RelayCommand _backArrowClickCommand;
        public ICommand BackArrowClick
        {
            get { return _backArrowClickCommand ?? (_backArrowClickCommand = new RelayCommand(BackArrowClickHandler)); }
        }

        private RelayCommand _refreshClickCommand;
        public ICommand RefreshClick
        {
            get { return _refreshClickCommand ?? (_refreshClickCommand = new RelayCommand(RefreshClickHandler)); }
        }
        #endregion
        public void AnalogClickHandler(string paramName, bool isChecked)
        {
            var plotService = IoC.Resolve<PlotService>();
            if (isChecked)
            {
                if (PlotModelAnalog.Series.Count == 6)
                {
                    ListCollectionAnalog.FirstOrDefault(f => f.Name == paramName).IsChecked = false;
                    MessageBox.Show("Максимальное колличество параметров = 6", "Ошибка", MessageBoxButton.OKCancel,
                                    MessageBoxImage.Error);
                    return;
                }
                plotService.ShowSignal(PlotModelAnalog, _fanObjectId, paramName, DateSelectedFrom, DateSelectedTo);
            }
            else
            {
                plotService.DeleteSignal(PlotModelAnalog, paramName);
            }
            PlotModelAnalog.RefreshPlot(true);
        }
        private void AllAnalogRefresh()
        {
            var plotService = IoC.Resolve<PlotService>();
            PlotModelAnalog = new PlotModel();
            plotService.SetUpModel(PlotModelAnalog, _fanObjectId, AnalogTitle, AnalogSubTitle);
            plotService.SetLinearDataAxis(PlotModelAnalog);
            foreach (var analogParam in ListCollectionAnalog)
            {
                if (analogParam.IsChecked)
                {
                    if (PlotModelAnalog.Series.Count == 6)
                    {
                        analogParam.IsChecked = false;
                        MessageBox.Show("Максимальное колличество параметров = 6", "Ошибка", MessageBoxButton.OKCancel,
                                        MessageBoxImage.Error);
                        return;
                    }
                    plotService.ShowSignal(PlotModelAnalog, _fanObjectId, analogParam.Name, DateSelectedFrom, DateSelectedTo);
                }   
            }
        }
        public void DigitClickHandler(string doorName, bool isChecked)
        {
            var plotService = IoC.Resolve<PlotService>();
            if (isChecked)
            {
                if (PlotModelDigit.Series.Count == 6)
                {
                    ListCollectionDigit.FirstOrDefault(f => f.Name == doorName).IsChecked = false;
                    MessageBox.Show("Максимальное колличество параметров = 6", "Ошибка", MessageBoxButton.OKCancel,
                                    MessageBoxImage.Error);
                    return;
                }
                plotService.ShowDoor(PlotModelDigit, _fanObjectId, doorName, DateSelectedFrom, DateSelectedTo);
            }
            else
            {
                plotService.DeleteSignal(PlotModelDigit, doorName);
            }
            PlotModelDigit.RefreshPlot(true);
        }
        private void AllDigitalRefresh()
        {
            var plotService = IoC.Resolve<PlotService>();
            PlotModelDigit = new PlotModel();
            plotService.SetUpModel(PlotModelDigit, _fanObjectId, DigitTiTle, DigitSubTitle);
            plotService.SetLinearDataAxis(PlotModelDigit);
            foreach (var digitParam in ListCollectionDigit)
            {
                if (digitParam.IsChecked)
                {
                    if (PlotModelDigit.Series.Count == 6)
                    {
                        digitParam.IsChecked = false;
                        MessageBox.Show("Максимальное колличество параметров = 6", "Ошибка", MessageBoxButton.OKCancel,
                                        MessageBoxImage.Error);
                        return;
                    }
                    plotService.ShowDoor(PlotModelDigit, _fanObjectId, digitParam.Name, DateSelectedFrom, DateSelectedTo);
                }
            }
        }
        private void BackArrowClickHandler()
        {
            IoC.Resolve<MainVm>().CurrentView = IoC.Resolve<GeneralVm>();
        }
        private void RefreshClickHandler()
        {
            AllAnalogRefresh();
            AllDigitalRefresh();
        }
        private void LoadDataInListBoxAnalog()
        {
            ListCollectionAnalog = new ObservableCollection<PlotData>();
            List<string> names = IoC.Resolve<DatabaseService>().GetAnalogSignalNames();
            names.ForEach(n => ListCollectionAnalog.Add(new PlotData { IsChecked = false, Name = n }));
            foreach (var checkBox in ListCollectionAnalog)
                checkBox.ClickEvent += AnalogClickHandler;
        }
        private void LoadDataInListBoxDigit()
        {
            ListCollectionDigit = new ObservableCollection<PlotData>();
            List<string> names = IoC.Resolve<DatabaseService>().GetDigitSignalNames();
            names.ForEach(n => ListCollectionDigit.Add(new PlotData { IsChecked = false, Name = n }));
            foreach (var checkBox in ListCollectionDigit)
                checkBox.ClickEvent += DigitClickHandler;
        }
        private void PlotsSetUp()
        {
            PlotModelAnalog = new PlotModel();
            PlotModelDigit = new PlotModel();
            var plotService = IoC.Resolve<PlotService>();
            plotService.SetUpModel(PlotModelAnalog, _fanObjectId, AnalogTitle, AnalogSubTitle);
            plotService.SetLinearDataAxis(PlotModelAnalog);
            plotService.SetUpModel(PlotModelDigit, _fanObjectId, DigitTiTle, DigitSubTitle);
            plotService.SetLinearDataAxis(PlotModelDigit);
            
        }
        private void DatePickersSetUp()
        {
            DateDisplayFrom = (DateTime.Now - new TimeSpan(24, 0, 0)).ToString();
            DateDisplayTo = DateTime.Now.ToString();
            DateSelectedFrom = DateTime.Now - new TimeSpan(300, 0, 0);
            DateSelectedTo = DateTime.Now;
        }
        public PlotVm(int fanObjectId,int parameterNum)
        {
            FanObjectId = fanObjectId;
            LoadDataInListBoxAnalog();
            LoadDataInListBoxDigit();
            DatePickersSetUp();
            PlotsSetUp();
            IoC.Resolve<PlotService>().ShowSignal(PlotModelAnalog, _fanObjectId, ListCollectionAnalog[parameterNum - 1].Name, DateSelectedFrom, DateSelectedTo);
            ListCollectionAnalog[parameterNum - 1].IsChecked = true;
        }
        public PlotVm(int fanObjectId = 1)
        {
            FanObjectId = fanObjectId;
            LoadDataInListBoxAnalog();
            LoadDataInListBoxDigit();
            DatePickersSetUp();
            PlotsSetUp();
        }
        private const string AnalogTitle = "График аналоговых параметров вентиляторная установка №";
        private const string AnalogSubTitle = "(Подробнее - CTRL + ЛКМ)";
        private const string DigitTiTle = "График цифровых параметров вентиляторная установка №";
        private const string DigitSubTitle = "('1' - открыт(а), '0' - закрыта(а), '0.5' - неопределенно)";

    }
}
