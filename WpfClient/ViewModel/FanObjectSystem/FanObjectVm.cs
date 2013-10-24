using System;
using System.Collections.Generic;
using System.Timers;
using System.Windows.Input;
using DataRepository.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Ninject.Parameters;
using WpfClient.Model;
using WpfClient.Model.Abstract;
using WpfClient.Model.Entities;
using WpfClient.Services;
using WpfClient.ViewModel.Plot;
using Parameter = WpfClient.Model.Entities.Parameter;

namespace WpfClient.ViewModel.FanObjectSystem
{
    class FanObjectVm : ViewModelBase, IDisposable, IUpDatable 
    {
        private readonly DatabaseService _databaseService;
        private readonly int _fanObjectId;
        private RelayCommand<object> _paramClickCommand;

        public TubeSystemVm TubeSystemVm { get; set; }
        public ThermometerVm ThermometerVm { get; set; }
        public IndicatorVm IndicatorVm { get; set; }
        public ICommand ParameterClick
        {
            get { return _paramClickCommand ?? (_paramClickCommand = new RelayCommand<object>(OnParamClick)); }
        }

        private RelayCommand<object> _fanClickCommand;
        public ICommand FanClick
        {
            get { return _fanClickCommand ?? (_fanClickCommand = new RelayCommand<object>(OnFanClick)); }
        }

        public FanObjectVm(int fanObjectId, DatabaseService databaseService, TubeSystemVm tubeSystemVm, IndicatorVm indicatorVm, ThermometerVm thermometerVm)
        {
            _databaseService = databaseService;
            _fanObjectId = fanObjectId;

            TubeSystemVm = tubeSystemVm;
            ThermometerVm = thermometerVm;
            IndicatorVm = indicatorVm;

            LoadFromDb();
        }
        public void Dispose()
        {
            TubeSystemVm.Dispose();
        }
        public void Update(FanLog fanLog)
        {
            var fanObject = _databaseService.GetFanObject(fanLog.FanNumber==_fanObjectId?fanLog:null);
            if (fanObject == null)
                return;
            TubeSystemVm.Update(fanObject);
            IndicatorVm.Update(getIndicatorValues(fanObject));
            ThermometerVm.Update(getThermometerValues(fanObject));
        }
        public void LoadFromDb()
        {
            var fanObject = _databaseService.GetFanObject(_fanObjectId);
            if(fanObject==null)
                return;
            TubeSystemVm.Update(fanObject);
            IndicatorVm.Update(getIndicatorValues(fanObject));
            ThermometerVm.Update(getThermometerValues(fanObject));
        }

        private List<Parameter> getThermometerValues(FanObject fanObject)
        {
            //show working fan number parameters
            if (fanObject.WorkingFanNumber == 2)
                return new List<Parameter>
                {
                    fanObject.Parameters[(int) WpfClient.Model.AnalogSignalType.mi46 - 1],
                    fanObject.Parameters[(int) WpfClient.Model.AnalogSignalType.mi47 - 1],
                    fanObject.Parameters[(int) WpfClient.Model.AnalogSignalType.mi48 - 1],
                    fanObject.Parameters[(int) WpfClient.Model.AnalogSignalType.mi49 - 1]
                };
            else
                return new List<Parameter>
                {
                    fanObject.Parameters[(int) WpfClient.Model.AnalogSignalType.mi6 - 1],
                    fanObject.Parameters[(int) WpfClient.Model.AnalogSignalType.mi25 - 1],
                    fanObject.Parameters[(int) WpfClient.Model.AnalogSignalType.mi26 - 1],
                    fanObject.Parameters[(int) WpfClient.Model.AnalogSignalType.mi27 - 1]
                };
        }

        private List<Parameter> getIndicatorValues(FanObject fanObject)
        {
            if (fanObject.WorkingFanNumber == 2)
                return new List<Parameter>
                {
                    fanObject.Parameters[(int) WpfClient.Model.AnalogSignalType.mi44 - 1],
                    fanObject.Parameters[(int) WpfClient.Model.AnalogSignalType.mi45 - 1],
                    fanObject.Parameters[(int) WpfClient.Model.AnalogSignalType.mi50 - 1],
                    fanObject.Parameters[(int) WpfClient.Model.AnalogSignalType.mi51 - 1],
                    fanObject.Parameters[(int) WpfClient.Model.AnalogSignalType.mi52 - 1],
                    fanObject.Parameters[(int) WpfClient.Model.AnalogSignalType.mi53 - 1]
                };
            else
                return new List<Parameter>
                {
                    fanObject.Parameters[(int) WpfClient.Model.AnalogSignalType.mi2 - 1],
                    fanObject.Parameters[(int) WpfClient.Model.AnalogSignalType.mi4 - 1],
                    fanObject.Parameters[(int) WpfClient.Model.AnalogSignalType.mi28 - 1],
                    fanObject.Parameters[(int) WpfClient.Model.AnalogSignalType.mi29 - 1],
                    fanObject.Parameters[(int) WpfClient.Model.AnalogSignalType.mi30 - 1],
                    fanObject.Parameters[(int) WpfClient.Model.AnalogSignalType.mi31 - 1]
                };
        }

        private void OnParamClick(object t)
        {
            PlotVm analogParametersVm;
            int paramIndex = int.Parse(t.ToString());
            var fanObject = _databaseService.GetFanObject(_fanObjectId);
            if (fanObject.WorkingFanNumber == 2)
            {
                paramIndex += (int)WpfClient.Model.AnalogSignalType.mi44 - 1;
                analogParametersVm = IoC.Resolve<PlotVm>(new ConstructorArgument("fanObjectId", _fanObjectId),
                    new ConstructorArgument("parameterNum", Int32.Parse(paramIndex.ToString())));
            }
            else
                analogParametersVm = IoC.Resolve<PlotVm>(new ConstructorArgument("fanObjectId", _fanObjectId),
                    new ConstructorArgument("parameterNum", Int32.Parse(t.ToString())));
            IDisposable dispose = (IDisposable)IoC.Resolve<MainVm>().CurrentView;
            dispose.Dispose();
            IoC.Resolve<MainVm>().CurrentView = analogParametersVm;
        }
        private void OnFanClick(object t)
        {
            IoC.Resolve<MainVm>().CurrentView = new ElectricDriveVm(this,_fanObjectId, int.Parse(t.ToString()));
        }
    }
}
