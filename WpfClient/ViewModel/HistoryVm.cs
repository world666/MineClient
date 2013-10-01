using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
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
    class HistoryVm : ViewModelBase, IDisposable
    {
        #region Property

        public ObservableCollection<OnPlotClickData> ListCollection { get; set; }

        private int _progressMax;
        public int ProgressMax
        {
            get { return _progressMax; }
            set { _progressMax = value; RaisePropertyChanged("ProgressMax"); }
        }

        private int _progressValue;
        public int ProgressValue
        {
            get { return _progressValue; }
            set { _progressValue = value; RaisePropertyChanged("ProgressValue"); }
        }

        private string _fanObjectId="1";
        public string FanObjectId
        {
            get { return _fanObjectId; }
            set { _fanObjectId = value; RaisePropertyChanged("FanObjectId"); }
        }

        private string _dateDisplayFrom = (DateTime.Now - new TimeSpan(24, 0, 0)).ToString();

        public string DateDisplayFrom
        {
            get { return _dateDisplayFrom; }
            set { _dateDisplayFrom = value; RaisePropertyChanged("DateDisplayFrom"); }
        }

        private DateTime _dateSelectedFrom;
        public DateTime DateSelectedFrom
        {
            get
            {
                return _dateSelectedFrom;
            }
            set
            {
                _dateSelectedFrom = value;
                RaisePropertyChanged("DateSelectedFrom");
            }
        }

        private string _dateDisplayTill = DateTime.Now.ToString();
        public string DateDisplayTill
        {
            get { return _dateDisplayTill; }
            set { _dateDisplayTill = value; RaisePropertyChanged("DateDisplayTill"); }
        }

        private DateTime _dateSelectedTill = DateTime.Now;
        public DateTime DateSelectedTill
        {
            get
            {
                return _dateSelectedTill;
            }
            set
            {
                _dateSelectedTill = value;
                RaisePropertyChanged("DateSelectedTill");
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

        
        private RelayCommand _backArrowClickCommand;
        public ICommand BackArrowClick
        {
            get { return _backArrowClickCommand ?? (_backArrowClickCommand = new RelayCommand(BackArrowClickHandler)); }
        }
        private RelayCommand _deletClickCommand;
        public ICommand DeleteClick
        {
            get { return _deletClickCommand ?? (_deletClickCommand = new RelayCommand(DeleteClickHandler)); }
        }
        private RelayCommand _deletAllClickCommand;
        public ICommand DeleteAllClick
        {
            get { return _deletAllClickCommand ?? (_deletAllClickCommand = new RelayCommand(DeleteAllClickHandler)); }
        }
        #endregion


        private void BackArrowClickHandler()
        {
            IDisposable dispose = (IDisposable)IoC.Resolve<MainVm>().CurrentView;
            dispose.Dispose();
            IoC.Resolve<MainVm>().CurrentView = IoC.Resolve<GeneralVm>();
        }
        private void DeleteClickHandler()
        {
            int curRec = 0;
            if (!Int32.TryParse(CurrentRecord,out curRec))
                return;
            var result = MessageBox.Show("Вы хотите удалить выбранную записи?", "Вопрос", MessageBoxButton.YesNo,
                                MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                IoC.Resolve<DatabaseService>().DeleteRecordById(_recordsId[curRec - 1]);
                ReadDataFromDB();
            }
        }

        private void DeleteAllClickHandler()
        {
            var result = MessageBox.Show("Вы хотите удалить все выбранные записи?", "Вопрос", MessageBoxButton.YesNo,
                                MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                ProgressMax = _recordsId.Count;
                Thread deleteThread = new Thread(DeleteThread);
                deleteThread.Start();
            }
        }
        private void DeleteThread()
        {
            for (int i = 0; i < _recordsId.Count; i++)
            {
                IoC.Resolve<DatabaseService>().DeleteRecordById(_recordsId[i]);
                ProgressValue = i;
            }
            MessageBox.Show("Все записи успешно удалены", "Информация", MessageBoxButton.OK,
                                MessageBoxImage.Information);
            ProgressMax = 1;
            ProgressValue = 0;
            Application.Current.Dispatcher.BeginInvoke(new Action(ReadDataFromDB));
            
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
            var dateNow = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            _dateSelectedFrom = dateNow - new TimeSpan(1, 0, 0);
            ProgressMax = 1;
            ProgressValue = 0;
        }
        public void Dispose()
        {}
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
                MessageBox.Show("Номер вентиляторной установки не может быть <= 0", "Ошибка", MessageBoxButton.OKCancel,
                                MessageBoxImage.Error);
                return false;
            }
            if (DateSelectedFrom > DateSelectedTill)
            {
                MessageBox.Show("Дата (от) не может быть быть больше времени (до)", "Ошибка", MessageBoxButton.OKCancel,
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
            var databaseService = IoC.Resolve<DatabaseService>();
            _recordsId = databaseService.HistoryGetRecordsCount(Int32.Parse(FanObjectId), DateSelectedFrom, DateSelectedTill);
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
