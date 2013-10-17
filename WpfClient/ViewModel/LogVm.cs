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
using WpfClient.Model.Concrete;
using WpfClient.Model.Plot;
using WpfClient.Services;
using WpfClient.ViewModel.General;

namespace WpfClient.ViewModel
{
    class LogVm : ViewModelBase, IDisposable
    {
        #region Property

        public ObservableCollection<RemoteLogData> ListCollection { get; set; }

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


        private string _dateDisplayFrom = (DateTime.Now - new TimeSpan(24, 0, 0)).ToString();

        public string DateDisplayFrom
        {
            get { return _dateDisplayFrom; }
            set { _dateDisplayFrom = value; RaisePropertyChanged("DateDisplayFrom"); }
        }

        private DateTime _dateSelectedFrom = (DateTime.Now - new TimeSpan(24, 0, 0));
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

        private int _selectedItem;
        public int SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value; RaisePropertyChanged("SelectedItem"); }
        }

        private RelayCommand _findClickCommand;
        public ICommand FindClick
        {
            get { return _findClickCommand ?? (_findClickCommand = new RelayCommand(FindClickHandler)); }
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
            var result = MessageBox.Show("Вы хотите удалить выбранную запись?", "Вопрос", MessageBoxButton.YesNo,
                                MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes && SelectedItem>=0)
            {
                IoC.Resolve<DatabaseService>().DeleteRemoteLogById(ListCollection[SelectedItem].Id);
                UpdateProperyValue();
            }
        }

        private void DeleteAllClickHandler()
        {
            var result = MessageBox.Show("Вы хотите удалить все выбранные записи?", "Вопрос", MessageBoxButton.YesNo,
                                MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                ProgressMax = ListCollection.Count;
                Thread deleteThread = new Thread(DeleteThread);
                deleteThread.Start();
            }
        }
        private void DeleteThread()
        {
            for (int i = 0; i < ListCollection.Count; i++)
            {
                IoC.Resolve<DatabaseService>().DeleteRemoteLogById(ListCollection[i].Id);
                ProgressValue = i;
            }
            MessageBox.Show("Все записи успешно удалены", "Информация", MessageBoxButton.OK,
                                MessageBoxImage.Information);
            ProgressMax = 1;
            ProgressValue = 0;
           Application.Current.Dispatcher.BeginInvoke(new Action(UpdateProperyValue));

        }
        public LogVm()
        {
            ListCollection = new ObservableCollection<RemoteLogData>();
            ProgressMax = 1;
            ProgressValue = 0;
        }

        public void Dispose()
        {
        }

        private void FindClickHandler()
        {
            if (CheckInput())
                UpdateProperyValue();
        }
        private bool CheckInput()
        {
            //ckeck fan number
            if (DateSelectedFrom > DateSelectedTill)
            {
                MessageBox.Show("Дата (от) не может быть быть больше времени (до)", "Ошибка", MessageBoxButton.OKCancel,
                                MessageBoxImage.Error);
                return false;
            }

            return true;
        }
        private void UpdateProperyValue()
        {
            
            _propertyList = IoC.Resolve<DatabaseService>().LogFind(DateSelectedFrom,DateSelectedTill);
            RecordsFound = _propertyList.Count.ToString();
            ListCollection.Clear();
            _propertyList.ForEach(n => ListCollection.Add(n));
        }
        private List<RemoteLogData> _propertyList;
    }
}
