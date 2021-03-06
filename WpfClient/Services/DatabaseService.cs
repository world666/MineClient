﻿using System;
using System.Collections.Generic;
using System.Linq;
using DataRepository.DataAccess.GenericRepository;
using DataRepository.Models;
using WpfClient.Model.Concrete;
using WpfClient.Model.Entities;
using WpfClient.Model.Plot;
using WpfClient.ViewModel.Plot;

namespace WpfClient.Services
{
    public class DatabaseService
    {
        public List<Parameter> GetAnalogSignals(int fanObjectNum)
        {
            var parameters = new List<Parameter>();

            using (var unit = new RepoUnit())
            {
                if (isFanLogEmpty(unit, fanObjectNum)) return null;
                var analogSignals = unit.FanLog.LastRecord(f => f.FanNumber == fanObjectNum).AnalogSignalLogs.ToList();
                parameters.AddRange(analogSignals.Select(signal => new Parameter {Name = signal.SignalType.Type, Value = signal.SignalValue}));
            }

            return parameters;
        }

        public List<string> GetAnalogSignalNames()
        {
            var names = new List<string>();

            using (var unit = new RepoUnit())
            {
                names.AddRange(unit.AnalogSignal.Load().Select(s => s.Type).ToList());
            }
            return names;
        }
        public List<string> GetDigitSignalNames()
        {
            var names = new List<string>();

            using (var unit = new RepoUnit())
            {
                names.AddRange(unit.DoorType.Load().Select(s => s.Type).ToList());
            }
            return names;
        }

        public FanObject GetFanObject(int fanOjbectNum)
        {
            try
            {
                using (var unit = new RepoUnit())
                {
                    if (isFanLogEmpty(unit, fanOjbectNum)) return null;

                    var fansLogRepo = unit.FanLog;

                    //var fansLogId = fansLogRepo.Load().Where(f => f.FanNumber == fanOjbectNum).Max(n => n.Id);
                    //var fanLog = fansLogRepo.Find(fansLogId);
                    var fanLog = fansLogRepo.LastRecord(f => f.FanNumber == fanOjbectNum);
                    return GetFanObject(fanLog);
                }
            }
            catch (Exception)
            {
                //nothing in db
            }
            return null;
        }

        public FanObject GetFanObject(FanLog fanLog)
        {
            if (fanLog == null)
                return null;
            var analogNames = GetAnalogSignalNames();
            var fanObjectVm = new FanObject { FanObjectId = fanLog.FanNumber };
            try
            {
                fanObjectVm.Parameters.AddRange(
                    fanLog.AnalogSignalLogs.Select(
                        s =>
                        new Parameter
                            {
                                Name = analogNames[s.SignalTypeId - 1],
                                Value = SystemStateService.GetLinearAnalogValue(analogNames[s.SignalTypeId - 1],s.SignalValue)
                            }));
                fanObjectVm.Parameters.ForEach(f => f.State = SystemStateService.GetParameterState(f.Name, f.Value));

                fanObjectVm.Doors.AddRange(
                    fanLog.DoorsLogs.Select(d => new Door {StateId = d.DoorStateId, TypeId = d.DoorTypeId}));
                fanObjectVm.WorkingFanNumber = GetWorkingFanNumber(fanLog.Fan1State_Id, fanLog.Fan2State_Id);
                fanObjectVm.Date = fanLog.Date;

            }
            catch (Exception)
            {
                //nothing in db
            }
            return fanObjectVm;
        }

        private bool isFanLogEmpty(RepoUnit unit, int fanObjectNum)
        {
            return unit == null || !unit.FanLog.Load(f => f.FanNumber == fanObjectNum).Any();
        }

        private int GetWorkingFanNumber(int? fan1State, int? fan2State)
        {
            return fan1State == fan2State ? 0 : fan1State == 2 ? 1 : 2;
        }


        public List<OnPlotClickData> FindDataByIdAndDate(int fanNum, DateTime date)//получить запись из архива по дате
        {
            var propertyList = new List<OnPlotClickData>();

            try
            {
                using (var repoUnit = new RepoUnit())
                {
                    var fansLogRepo = repoUnit.FanLog;

                    var fansLogId = fansLogRepo.Load().Where(n => n.FanNumber == fanNum && n.Date == date).Max(n => n.Id);
                    var fansLog = fansLogRepo.Find(fansLogId);

                    propertyList.Add(new OnPlotClickData { Property = "Время приема параметров", Value = fansLog.Date.ToString() });
                    propertyList.Add(new OnPlotClickData { Property = "Вентиляторная установка №", Value = fanNum.ToString() });
                    propertyList.Add(new OnPlotClickData { Property = "Вентилятор 1", Value = fansLog.Fan1State.State });
                    propertyList.Add(new OnPlotClickData { Property = "Вентилятор 2", Value = fansLog.Fan2State.State });

                    propertyList.AddRange(fansLog.DoorsLogs.Select(doorLog => new OnPlotClickData { Property = doorLog.DoorType.Type, Value = doorLog.DoorState.State }));
                    propertyList.AddRange(fansLog.AnalogSignalLogs.Select(signal => new OnPlotClickData { Property = signal.SignalType.Type, Value = SystemStateService.GetLinearAnalogValue(signal.SignalType.Type, signal.SignalValue).ToString() }));
                }
            }
            catch (Exception)
            {
                //nothing in Db
            }
            return propertyList;
        }

        public List<OnPlotClickData> HistoryFind(int fanLogId)//получить запись архива по идентификатору
        {
            var propertyList = new List<OnPlotClickData>();

            try
            {
                using (var repoUnit = new RepoUnit())
                {
                    var fansLogRepo = repoUnit.FanLog;
                    var fansLog = fansLogRepo.Load().FirstOrDefault(n=> n.Id == fanLogId);
                    propertyList.Add(new OnPlotClickData { Property = "Время приема параметров", Value = fansLog.Date.ToString() });
                    propertyList.Add(new OnPlotClickData { Property = "Вентиляторная установка №", Value = fansLog.FanNumber.ToString()});
                    propertyList.Add(new OnPlotClickData { Property = "Вентилятор 1", Value = fansLog.Fan1State.State });
                    propertyList.Add(new OnPlotClickData { Property = "Вентилятор 2", Value = fansLog.Fan2State.State });

                    propertyList.AddRange(fansLog.DoorsLogs.Select(doorLog => new OnPlotClickData { Property = doorLog.DoorType.Type, Value = doorLog.DoorState.State }));
                    propertyList.AddRange(fansLog.AnalogSignalLogs.Select(signal => new OnPlotClickData { Property = signal.SignalType.Type, Value = SystemStateService.GetLinearAnalogValue(signal.SignalType.Type, signal.SignalValue).ToString() }));
                }
            }
            catch (Exception)
            {
                //nothing in Db
            }
            return propertyList;
        }
        public List<int> HistoryGetRecordsCount(int fanNum, DateTime dateFrom, DateTime dateTill)//получить колличество записей архива по дате
        {
            using (var repoUnit = new RepoUnit())
            {
                var fansLogRepo = repoUnit.FanLog;
                var fansLogId =
                    fansLogRepo.Load()
                               .Where(n => n.FanNumber == fanNum && n.Date >= dateFrom && n.Date <= dateTill)
                               .Select(n => n.Id);
                return fansLogId.ToList();
            }
        }
        public void DeleteRecordById(int id) //удалить из архива запись
        {
            using (var repoUnit = new RepoUnit())
            {
                FanLog fansLog = repoUnit.FanLog.Find(id);
                if (fansLog == null) return;
                repoUnit.FanLog.Delete(fansLog);
                repoUnit.FanLog.SaveChanges();
            }
        }
        public void DeleteRecordById(List<int> id)//удалить из архива по списку
        {
            if (id==null) return;
            using (var repoUnit = new RepoUnit())
            {
                foreach (var i in id)
                {
                    FanLog fansLog = repoUnit.FanLog.Find(i);
                    if(fansLog==null) continue;
                    repoUnit.FanLog.Delete(fansLog);
                }
                repoUnit.FanLog.SaveChanges();
            }
        }
        public List<RemoteLogData> LogFind(DateTime dateFrom, DateTime dateTill)//вернуть журнал удаленного управления по дате 
        {
            var remoteList = new List<RemoteLogData>();
            using (var repoUnit = new RepoUnit())
            {
                var remoteLogRepo = repoUnit.RemoteLog;
                var remoteLogs =
                    remoteLogRepo.Load()
                               .Where(n => n.Date >= dateFrom && n.Date <= dateTill);
                remoteList.AddRange(remoteLogs.Select(f => new RemoteLogData {Id = f.Id, Date = f.Date, FanObjectNum = f.FanObjectNum, Person = f.Person,RemoteCommand = f.RemoteState.State, State = f.Sent == true ? "Выполнено" : "Не выполнено"}));
                return remoteList;
            }
        }
        public void DeleteRemoteLogById(int id)//удалить запись из журнала удаленного управления
        {
            using (var repoUnit = new RepoUnit())
            {
                RemoteLog remoteLog = repoUnit.RemoteLog.Find(id);
                if (remoteLog == null) return;
                repoUnit.RemoteLog.Delete(remoteLog);
                repoUnit.RemoteLog.SaveChanges();
            }
        }
    }
}
