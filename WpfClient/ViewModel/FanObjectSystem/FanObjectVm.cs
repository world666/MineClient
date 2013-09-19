using System;
using System.Collections.Generic;
using System.Timers;
using GalaSoft.MvvmLight;
using WpfClient.Model;
using WpfClient.Model.Entities;
using WpfClient.Services;

namespace WpfClient.ViewModel.FanObjectSystem
{
    class FanObjectVm : ViewModelBase, IDisposable
    {
        private readonly DatabaseService _databaseService;
        private readonly int _fanObjectId;
        private Timer timer;

        public TubeSystemVm TubeSystemVm { get; set; }
        public ThermometerVm ThermometerVm { get; set; }
        public IndicatorVm IndicatorVm { get; set; }

        public FanObjectVm(int fanObjectId, DatabaseService databaseService, TubeSystemVm tubeSystemVm, IndicatorVm indicatorVm, ThermometerVm thermometerVm)
        {
            _databaseService = databaseService;
            _fanObjectId = fanObjectId;

            TubeSystemVm = tubeSystemVm;
            ThermometerVm = thermometerVm;
            IndicatorVm = indicatorVm;

            Update();

            timer = new Timer(10000);
            timer.Elapsed += (sender, args) => Update();
            timer.Start();
        }
        public void Dispose()
        {
            timer.Stop();
            timer.Dispose();
        }
        public void Update()
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
            return new List<Parameter> {fanObject.Parameters[2]};
        }

        private List<Parameter> getIndicatorValues(FanObject fanObject)
        {
            return new List<Parameter> { fanObject.Parameters[0], fanObject.Parameters[1]};
        }
    }
}
