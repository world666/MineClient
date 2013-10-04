using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataRepository.DataAccess.GenericRepository;
using Mc.MvcWeb.Models;

namespace Mc.MvcWeb.Services
{
    public class DatabaseService
    {
        public FanObject GetFanObject(int fanOjbectNum)
        {
            var fanObjectVm = new FanObject { FanObjectId = fanOjbectNum };
            try
            {
                using (var unit = new RepoUnit())
                {
                    if (isFanLogEmpty(unit, fanOjbectNum)) return null;

                    var fansLogRepo = unit.FanLog;

                    //var fansLogId = fansLogRepo.Load().Where(f => f.FanNumber == fanOjbectNum).Max(n => n.Id);
                    //var fanLog = fansLogRepo.Find(fansLogId);
                    var fanLog = fansLogRepo.LastRecord(f => f.FanNumber == fanOjbectNum);
                    fanObjectVm.Parameters.AddRange(
                        fanLog.AnalogSignalLogs.Select(
                            s =>
                            new Parameter
                            {
                                Name = s.SignalType.Type,
                                Value = SystemStateService.GetLinearAnalogValue(s.SignalType.Type, s.SignalValue).ToString()
                            }));
                    fanObjectVm.Parameters.ForEach(f => f.State = SystemStateService.GetParameterState(f.Name, double.Parse(f.Value)));

                    fanObjectVm.Doors.AddRange(
                        fanLog.DoorsLogs.Select(d => new Door { StateId = d.DoorStateId, TypeId = d.DoorTypeId }));
                    fanObjectVm.WorkingFanNumber = GetWorkingFanNumber(fanLog.Fan1State_Id, fanLog.Fan2State_Id);
                    fanObjectVm.Date = fanLog.Date;
                }
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
    }
}