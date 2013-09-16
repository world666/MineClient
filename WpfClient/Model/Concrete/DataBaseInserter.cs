using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataRepository.DataAccess.GenericRepository;
using DataRepository.Models;
using WpfClient.Model.Abstract;
using WpfClient.Services;

namespace WpfClient.Model.Concrete
{
    class DataBaseInserter : IDataInserter
    {
        private readonly IMsgParser _msgParser;


        public DataBaseInserter(IMsgParser msgParser)
        {
            _msgParser = msgParser;
        }

        public void InsertRawData(string message) {
            
        }

        public void InsertData(string data)
        {
            var paramsTable = _msgParser.Parse(data);

            using (var repoUnit = new RepoUnit()) 
            {
                repoUnit.FanLog.Save(MapToFanLog(paramsTable));
                repoUnit.Commit();
            }
        }

        public List<DoorsLog> MapToDoorsLog(IDictionary<string, int> paramsTable)
        {
            return (from object doorEnum in Enum.GetValues(typeof (DoorType)) 
                    select new DoorsLog {DoorTypeId = (int) doorEnum, DoorStateId = paramsTable[doorEnum.ToString()]}).ToList();
        }

        public List<AnalogSignalLog> MapToAnalogSignalLog(IDictionary<string, int> paramsTable)
        {
            return (from object analogEnum in Enum.GetValues(typeof(AnalogSignalType)) 
                    select new AnalogSignalLog {SignalTypeId = (int) analogEnum, SignalValue = paramsTable[analogEnum.ToString()]}).ToList();
        }

        public FanLog MapToFanLog(IDictionary<string, int> paramsTable) {

            return new FanLog {
                FanNumber = paramsTable[FanEnum.fn.ToString()],
                Fan1State_Id = paramsTable[FanEnum.mb11.ToString()],
                Fan2State_Id = paramsTable[FanEnum.mb12.ToString()],
                Date = DateTime.Now,
                AnalogSignalLogs = MapToAnalogSignalLog(paramsTable),
                DoorsLogs = MapToDoorsLog(paramsTable)
            };
        }
    }
}
