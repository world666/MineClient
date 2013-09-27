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
            return new List<Parameter> { fanObject.Parameters[2], fanObject.Parameters[3], fanObject.Parameters[4], fanObject.Parameters[5] };
        }

        private List<Parameter> getIndicatorValues(FanObject fanObject)
        {

            return new List<Parameter> { fanObject.Parameters[0], fanObject.Parameters[1], fanObject.Parameters[6], fanObject.Parameters[7], fanObject.Parameters[8], fanObject.Parameters[9] };
        }

        private void OnParamClick(object t)
        {
            var analogParametersVm = IoC.Resolve<PlotVm>(new ConstructorArgument("fanObjectId", _fanObjectId), new ConstructorArgument("parameterNum", Int32.Parse(t.ToString())));
            IDisposable dispose = (IDisposable)IoC.Resolve<MainVm>().CurrentView;
            dispose.Dispose();
            IoC.Resolve<MainVm>().CurrentView = analogParametersVm;
        }
    }
}
