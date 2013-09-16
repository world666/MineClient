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
using WpfClient.Model;
using WpfClient.Model.Plot;
using WpfClient.Services;
using WpfClient.ViewModel.General;
using WpfClient.ViewModel.Plot;

namespace WpfClient.ViewModel
{
    class HistoryVm : ViewModelBase
    {
        #region Property
        private DateTime _dateTimeFrom = DateTime.Now;
        public DateTime DateTimeFrom
        {
            get
            {
                return _dateTimeFrom;
            }
            set
            {
                _dateTimeFrom = value;
                RaisePropertyChanged("DateTimeFrom");
            }
        }

        private DateTime _dateTimeTill = DateTime.Now;
        public DateTime DateTimeTill
        {
            get
            {
                return _dateTimeTill;
            }
            set
            {
                _dateTimeTill = value;
                RaisePropertyChanged("DateTimeTill");
            }
        }

        private string _fanObjectId="1";
        public string FanObjectId
        {
            get { return _fanObjectId; }
            set { _fanObjectId = value; RaisePropertyChanged("FanObjectId"); }
        }

        private string _dateDisplay = DateTime.Now.ToString();
        public string DateDisplay
        {
            get { return _dateDisplay; }
            set { _dateDisplay = value; RaisePropertyChanged("DateDisplay");}
        }

        private DateTime _dateSelected = DateTime.Now;
        public DateTime DateSelected
        {
            get
            {
                return _dateSelected;
            }
            set
            {
                _dateSelected = value;
                RaisePropertyChanged("DateSelected");
            }
        }

        private string _recordsFound = "0";
        public string RecordsFound
        {
            get { return _recordsFound; }
            set { _recordsFound = value; RaisePropertyChanged("RecordsFound"); }
        }

        private string _currentRecord;
        public string CurrentRecord
        {
            get { return _currentRecord; }
            set { _currentRecord = value; RaisePropertyChanged("CurrentRecord"); }
        }

        private RelayCommand _findClickCommand;
        public ICommand FindClick
        {
            get { return _findClickCommand ?? (_findClickCommand = new RelayCommand(FindClickHandler)); }
        }

        private RelayCommand _nextRecordClickCommand;
        public ICommand NextRecordClick
        {
            get { return _nextRecordClickCommand ?? (_nextRecordClickCommand = new RelayCommand(NextRecordClickHandler)); }
        }

        private RelayCommand _prevRecordClickCommand;
        public ICommand PrevRecordClick
        {
            get { return _prevRecordClickCommand ?? (_prevRecordClickCommand = new RelayCommand(PrevRecordClickHandler)); }
        }

        private RelayCommand _showRecordClickCommand;
        public ICommand ShowRecordClick
        {
            get { return _showRecordClickCommand ?? (_showRecordClickCommand = new RelayCommand(ShowRecordClickHandler)); }
        }

        public ObservableCollection<OnPlotClickData> ListCollection { get; set; }
        #endregion


        private RelayCommand _backArrowClickCommand;
        public ICommand BackArrowClick
        {
            get { return _backArrowClickCommand ?? (_backArrowClickCommand = new RelayCommand(BackArrowClickHandler)); }
        }

        private void BackArrowClickHandler()
        {
            IoC.Resolve<MainVm>().CurrentView = IoC.Resolve<GeneralVm>();
        }

        private void FindClickHandler()
        {
            if(CheckInput())
                ReadDataFromDB();
        }
        private void NextRecordClickHandler()
        {
            if (!CheckShowInput())
                return;
            var curRec = Int32.Parse(CurrentRecord);
            CurrentRecord = (++curRec).ToString();
            UpdateProperyValue();
        }

        private void PrevRecordClickHandler()
        {
            if (!CheckShowInput())
                return;
            var curRec = Int32.Parse(CurrentRecord);
            CurrentRecord = (--curRec).ToString();
            UpdateProperyValue();
        }

        private void ShowRecordClickHandler()
        {
            if (CheckShowInput())
                UpdateProperyValue();
        }


        public HistoryVm()
        {
            ListCollection = new ObservableCollection<OnPlotClickData>();
        }
        private bool CheckInput()
        {
            //ckeck fan number
            int _iFanObjectId;
            if (!Int32.TryParse(FanObjectId, out _iFanObjectId))
            {
                MessageBox.Show("Неверно указан номер вентиляторной установки", "Ошибка", MessageBoxButton.OKCancel,
                                MessageBoxImage.Error);
                return false;
            }
            if (_iFanObjectId <= 0)
            {
                MessageBox.Show("Номер вентиляторной установки не может быть >= 0", "Ошибка", MessageBoxButton.OKCancel,
                                MessageBoxImage.Error);
                return false;
            }
            if (DateTimeFrom > DateTimeTill)
            {
                MessageBox.Show("Время (от) не может быть быть больше времени (до)", "Ошибка", MessageBoxButton.OKCancel,
                                MessageBoxImage.Error);
                return false;
            }

            return true;
        }
        private bool CheckShowInput()
        {
            int intCurrentRecord;
            if (!Int32.TryParse(CurrentRecord, out intCurrentRecord))
            {
                MessageBox.Show("Неверно указан номер текущей записи", "Ошибка", MessageBoxButton.OKCancel,
                                MessageBoxImage.Error);
                return false;
            }
            if (_propertyList.Count <= 0)
                return false;
            return true;
        }
        private void ReadDataFromDB()
        {
            DateTime dateFrom = new DateTime(DateSelected.Year, DateSelected.Month, DateSelected.Day, DateTimeFrom.Hour, DateTimeFrom.Minute, DateTimeFrom.Second);
            DateTime dateTill = new DateTime(DateSelected.Year, DateSelected.Month, DateSelected.Day, DateTimeTill.Hour, DateTimeTill.Minute, DateTimeTill.Second);
            var databaseService = IoC.Resolve<DatabaseService>();
            _recordsId = databaseService.HistoryGetRecordsCount(Int32.Parse(FanObjectId), dateFrom, dateTill);
            if (_recordsId.Count <= 0)
            {
                CurrentRecord = "";
                RecordsFound = "0";
                ListCollection.Clear();
                return;
            } 
            CurrentRecord = "1";
            RecordsFound = _recordsId.Count.ToString();
            UpdateProperyValue();
        }
        private void UpdateProperyValue()
        {
            if (_recordsId.Count <= 0) 
                return;
            if (Int32.Parse(CurrentRecord) > Int32.Parse(RecordsFound))
                CurrentRecord = "1";
            if (Int32.Parse(CurrentRecord) <= 0)
                CurrentRecord = RecordsFound;
            _propertyList = IoC.Resolve<DatabaseService>().HistoryFind(_recordsId[Int32.Parse(CurrentRecord) - 1]);
            ListCollection.Clear();
            _propertyList.ForEach(n => ListCollection.Add(n));
        }
        private List<OnPlotClickData> _propertyList;
        private List<int> _recordsId;
    }
}
