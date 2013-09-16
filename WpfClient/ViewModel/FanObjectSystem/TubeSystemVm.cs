using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using GalaSoft.MvvmLight.Command;
using Mc.CustomControls.Model;
using WpfClient.Model;
using System.Linq;
using WpfClient.Services;
using WpfClient.Model.Entities;
using WpfClient.ViewModel.General;

namespace WpfClient.ViewModel.FanObjectSystem
{
    class TubeSystemVm : INotifyPropertyChanged
    {
        private ParameterVm _systemState;
        private readonly FanService _fanService;
        private int _fanObjectId;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<DoorStateEnum> DoorsState { get; set; }
        public ObservableCollection<string> DoorsText { get; set; }

        public DateTimeVm DateTime { get { return IoC.Resolve<DateTimeVm>(); } }

        private RelayCommand _backArrowClickCommand;
        public ICommand BackArrowClick
        {
            get { return _backArrowClickCommand ?? (_backArrowClickCommand = new RelayCommand(BackArrowClickHandler)); }
        }

        private void BackArrowClickHandler()
        {
            IoC.Resolve<MainVm>().CurrentView = IoC.Resolve<GeneralVm>();
        }
 
        #region Property
        #region FanProperty
        private bool _rotationV1;
        public bool RotationV1
        {
            get { return _rotationV1; }
            set { _rotationV1 = value; OnPropertyChanged("RotationV1"); }
        }
        private bool _rotationV2;
        public bool RotationV2
        {
            get { return _rotationV2; }
            set { _rotationV2 = value; OnPropertyChanged("RotationV2"); }
        }

        private string _firstFanOnOffMode;
        public string FirstFanOnOffMode
        {
            get { return _firstFanOnOffMode; }
            set { _firstFanOnOffMode = value; OnPropertyChanged("FirstFanOnOffMode"); }
        }

        private string _secondFanOnOffMode;
        public string SecondFanOnOffMode
        {
            get { return _secondFanOnOffMode; }
            set { _secondFanOnOffMode = value; OnPropertyChanged("SecondFanOnOffMode"); }
        }

        private RelayCommand<object> _firstFanClickCommand; //on/off fan
        public ICommand FirstFanClick
        {
             get { return _firstFanClickCommand ?? (_firstFanClickCommand = new RelayCommand<object>(FirstFanClickHandler)); }
        }

        private RelayCommand<object> _secondFanClickCommand; //on/off fan
        public ICommand SecondFanClick
        {
            get { return _secondFanClickCommand ?? (_secondFanClickCommand = new RelayCommand<object>(SecondFanClickHandler)); }
        }
        #endregion
        #region DoorProperty
        private DoorStateEnum _lo1;
        public DoorStateEnum Lo1
        {
            get { return _lo1; }
            set { _lo1 = value; OnPropertyChanged("Lo1"); }
        }
        private DoorStateEnum _lo2;
        public DoorStateEnum Lo2
        {
            get { return _lo2; }
            set { _lo2 = value; OnPropertyChanged("Lo2"); }
        }
        private DoorStateEnum _lp1;
        public DoorStateEnum Lp1
        {
            get { return _lp1; }
            set { _lp1 = value; OnPropertyChanged("Lp1"); }
        }
        private DoorStateEnum _lp2;
        public DoorStateEnum Lp2
        {
            get { return _lp2; }
            set { _lp2 = value; OnPropertyChanged("Lp2"); }
        }
        private DoorStateEnum _na1;
        public DoorStateEnum Na1
        {
            get { return _na1; }
            set { _na1 = value; OnPropertyChanged("Na1"); }
        }
        private DoorStateEnum _na2;
        public DoorStateEnum Na2
        {
            get { return _na2; }
            set { _na2 = value; OnPropertyChanged("Na2"); }
        }
        private DoorStateEnum _la;
        public DoorStateEnum La
        {
            get { return _la; }
            set { _la = value; OnPropertyChanged("La"); }
        }
        private DoorStateEnum _lpk;
        public DoorStateEnum Lpk
        {
            get { return _lpk; }
            set { _lpk = value; OnPropertyChanged("Lpk"); }
        }
        private DoorStateEnum _ld;
        public DoorStateEnum Ld
        {
            get { return _ld; }
            set { _ld = value; OnPropertyChanged("Ld"); }
        }
        #endregion
        #region TubeProperty
        private string _v1onDown;
        public string V1onDown
        {
            get { return _v1onDown; }
            set { _v1onDown = value; OnPropertyChanged("V1onDown"); }
        }
        private string _v1onTop;
        public string V1onTop
        {
            get { return _v1onTop; }
            set { _v1onTop = value; OnPropertyChanged("V1onTop"); }
        }
        private string _v1onLeft;
        public string V1onLeft
        {
            get { return _v1onLeft; }
            set { _v1onLeft = value; OnPropertyChanged("V1onLeft"); }
        }
        private string _v1onReight;
        public string V1onReight
        {
            get { return _v1onReight; }
            set { _v1onReight = value; OnPropertyChanged("V1onReight"); }
        }

        private string _v2onDown;
        public string V2onDown
        {
            get { return _v2onDown; }
            set { _v2onDown = value; OnPropertyChanged("V2onDown"); }
        }
        private string _v2onTop;
        public string V2onTop
        {
            get { return _v2onTop; }
            set { _v2onTop = value; OnPropertyChanged("V2onTop"); }
        }
        private string _v2onLeft;
        public string V2onLeft
        {
            get { return _v2onLeft; }
            set { _v2onLeft = value; OnPropertyChanged("V2onLeft"); }
        }
        private string _v2onReight;
        public string V2onReight
        {
            get { return _v2onReight; }
            set { _v2onReight = value; OnPropertyChanged("V2onReight"); }
        }

        private string _workReight;
        public string WorkReight
        {
            get { return _workReight; }
            set { _workReight = value; OnPropertyChanged("WorkReight"); }
        }
        private string _workLeft;
        public string WorkLeft
        {
            get { return _workLeft; }
            set { _workLeft = value; OnPropertyChanged("WorkLeft"); }
        }

        
        private string _normaTop;
        public string NormaTop
        {
            get { return _normaTop; }
            set { _normaTop = value; OnPropertyChanged("NormaTop"); }
        }
        private string _normaLeft;
        public string NormaLeft
        {
            get { return _normaLeft; }
            set { _normaLeft = value; OnPropertyChanged("NormaLeft"); }
        }

        private string _normaLeftReversReight;
        public string NormaLeftReversReight
        {
            get { return _normaLeftReversReight; }
            set { _normaLeftReversReight = value; OnPropertyChanged("NormaLeftReversReight"); }
        }
        private string _normaReightReversLeft;
        public string NormaReightReversLeft
        {
            get { return _normaReightReversLeft; }
            set { _normaReightReversLeft = value; OnPropertyChanged("NormaReightReversLeft"); }
        }

        private string _reversLeft;
        public string ReversLeft
        {
            get { return _reversLeft; }
            set { _reversLeft = value; OnPropertyChanged("ReversLeft"); }
        }

        private string _reversDown;
        public string ReversDown
        {
            get { return _reversDown; }
            set { _reversDown = value; OnPropertyChanged("ReversDown"); }
        }

        private string _normaTopReverseDown;
        public string NormaTopReverseDown {
            get { return _normaTopReverseDown; }
            set { _normaTopReverseDown = value; OnPropertyChanged("NormaTopReverseDown"); }
        }
        #endregion

        private bool _passwordBoxVisibilityFan1;
        public bool PasswordBoxVisibilityFan1
        {
            get { return _passwordBoxVisibilityFan1; }
            set { _passwordBoxVisibilityFan1 = value; OnPropertyChanged("PasswordBoxVisibilityFan1"); }
        }

        private bool _passwordBoxVisibilityFan2;
        public bool PasswordBoxVisibilityFan2
        {
            get { return _passwordBoxVisibilityFan2; }
            set { _passwordBoxVisibilityFan2 = value; OnPropertyChanged("PasswordBoxVisibilityFan2"); }
        }

        #endregion

        #region PrivateMethods
        private void ClearTubes()
        {
            ReversDown = "Stop";
            ReversLeft = "Stop";
            NormaReightReversLeft = "Stop";
            NormaLeftReversReight = "Stop";
            NormaTopReverseDown = "Stop";
            NormaLeft = "Stop";
            NormaTop = "Stop";
            WorkLeft = "Stop";
            WorkReight = "Stop";
            V1onDown = "Stop";
            V1onLeft = "Stop";
            V1onReight = "Stop";
            V1onTop = "Stop";
            V2onDown = "Stop";
            V2onLeft = "Stop";
            V2onReight = "Stop";
            V2onTop = "Stop";
        }
        private void V1Work()
        {
            V1onDown = "Down";
            V1onLeft = "Left";
            V1onReight = "Reight";
            V1onTop = "Top";
        }
        private void V2Work()
        {
            V2onDown = "Down";
            V2onLeft = "Left";
            V2onReight = "Reight";
            V2onTop = "Top";
        }
        private void Norma()
        {
            NormaLeft = "Left";
            NormaTop = "Top";
            NormaLeftReversReight = "Left";
            NormaReightReversLeft = "Reight";
            NormaTopReverseDown = "Top";
            WorkLeft = "Left";
            WorkReight = "Reight";
        }
        private void Revers()
        {
            ReversDown = "Down";
            ReversLeft = "Left";
            NormaLeftReversReight = "Reight";
            NormaReightReversLeft = "Left";
            NormaTopReverseDown = "Down";
            WorkLeft = "Left";
            WorkReight = "Reight";
        }
        private void FirstFanClickHandler(object password)
        {

            if (FirstFanOnOffMode != "ОК")
            {
                PasswordBoxVisibilityFan1 = true;
                PasswordBoxVisibilityFan2 = false;
                FirstFanOnOffMode = "ОК";
            }
            else
            {
                PasswordBox psd = password as PasswordBox;
                PasswordBoxVisibilityFan1 = false;
                FirstFanOnOffMode = RotationV1 == true ? "Отключить" : "Включить";
                if (psd.Password == "1111")
                {
                    RemouteFanControlService.DataForSending.FirstOrDefault(f => f.FanNum == _fanObjectId).Data =
                        FirstFanOnOffMode == "Включить" ? RemouteFanState.OnFan1 : RemouteFanState.Off;
                    MessageBox.Show("Пароль введен верно", "Информация", MessageBoxButton.OK,
                               MessageBoxImage.Information);
                }
                else
                    MessageBox.Show("Пароль введен неверно", "Ошибка", MessageBoxButton.OK,
                               MessageBoxImage.Error);
                psd.Password = "";
            }
        }

        private void SecondFanClickHandler(object password)
        {
            if (SecondFanOnOffMode != "ОК")
            {
                PasswordBoxVisibilityFan2 = true;
                PasswordBoxVisibilityFan1 = false;
                SecondFanOnOffMode = "ОК";
            }
            else
            {
                PasswordBox psd = password as PasswordBox;
                PasswordBoxVisibilityFan2 = false;
                SecondFanOnOffMode = RotationV2 == true ? "Отключить" : "Включить";
                if (psd.Password == "1111")
                {
                    RemouteFanControlService.DataForSending.FirstOrDefault(f => f.FanNum == _fanObjectId).Data =
                        SecondFanOnOffMode == "Включить" ? RemouteFanState.OnFan2 : RemouteFanState.Off;
                    MessageBox.Show("Пароль введен верно", "Информация", MessageBoxButton.OK,
                              MessageBoxImage.Information);
                }
                else
                    MessageBox.Show("Пароль введен неверно", "Ошибка", MessageBoxButton.OK,
                               MessageBoxImage.Error);
                psd.Password = "";
            }


        }

        #endregion PrivateMethods

        public ParameterVm SystemState
        {
            get { return _systemState ?? (_systemState = new ParameterVm()); }
            set
            {
                _systemState = value;
                OnPropertyChanged("SystemState");
            }
        }

        public TubeSystemVm(FanService fanService)
        {
            ClearTubes();
            _fanService = fanService;

            DoorsState = new ObservableCollection<DoorStateEnum>();
            for (int i = 0; i < 15; i++)
                DoorsState.Add(DoorStateEnum.Open);
            DoorsText = new ObservableCollection<string>();
            for (int i = 0; i < 15; i++)
                DoorsText.Add("WhiteSmoke");
            PasswordBoxVisibilityFan1 = false;
            PasswordBoxVisibilityFan2 = false;
        }

        public void Update(FanObject fanObject)
        {
            _fanObjectId = fanObject.FanObjectId;
            setFanMode(fanObject);
            setWorkingFan(fanObject.WorkingFanNumber);
            setDoorsState(fanObject.Doors);
            setOnOffModeState();
        }     

        private void setWorkingFan(int workingFan)
        {
            RotationV1 = workingFan == 1;
            RotationV2 = workingFan == 2;
            setArrows(workingFan);
        }

        private void setArrows(int workingFan) 
        {
            ClearTubes();
            switch (SystemState.Value) 
            {
                case "Реверс": Revers(); break;
                case "Норма": Norma(); break;
            }
            if (SystemState.Value == "Реверс" || SystemState.Value == "Норма")
            {
                if (workingFan == 1) V1Work();
                if (workingFan == 2) V2Work();
            }

        }

        private void setFanMode(FanObject fanObject)
        {
            var value = new StringBuilder();
            var state = StateEnum.Ok;

            //Get if reverse or normal
            var fanMode = _fanService.GetFanMode(fanObject.WorkingFanNumber, fanObject.Doors);
            state = fanMode.State;
            value.Append(fanMode.Value);

            //Check if working fan number is correct
            var fanState = SystemStateService.GetFanState(fanObject.WorkingFanNumber);

            if (fanState == StateEnum.Dangerous || fanMode.State == StateEnum.Dangerous)
            {
                state = StateEnum.Dangerous;
                if (fanState == StateEnum.Dangerous) value.Append("\nВентилятор остановлен");
                else if (fanMode.State == StateEnum.Dangerous) value.Append("\nЛяды не собраны");
            }

            SystemState = new ParameterVm { Value = value.ToString(), State = state };
        }

        private void setDoorsState(List<Door> doors)
        {
            foreach (var type in Enum.GetValues(typeof (DoorType)))
            {
                var stateId = doors.First(d => d.TypeId == (int) type).StateId;
                DoorsState[(int) type] = Enum.IsDefined(typeof(DoorStateEnum), stateId) ? (DoorStateEnum)stateId : DoorStateEnum.Undefined ;
                DoorsText[(int) type] = DoorsState[(int) type] == DoorStateEnum.Undefined
                                            ? "Red"
                                            : "WhiteSmoke";
            }
        }

        private void setOnOffModeState()
        {
            FirstFanOnOffMode = RotationV1 == true ? "Отключить" : "Включить";
            SecondFanOnOffMode = RotationV2 == true ? "Отключить" : "Включить";
            if (PasswordBoxVisibilityFan1)
                FirstFanOnOffMode = "ОК";
            if (PasswordBoxVisibilityFan2)
                SecondFanOnOffMode = "ОК";
        }
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
