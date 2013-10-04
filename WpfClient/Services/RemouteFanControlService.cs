using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataRepository.DataAccess.GenericRepository;
using DataRepository.Models;
using WpfClient.Model;
using WpfClient.Model.Concrete;

namespace WpfClient.Services
{
    public static class RemouteFanControlService
    {
        public static void SetData(int fanObjectId, RemouteFanState dataForSending)
        {
            using (var repoUnit = new RepoUnit())
            {
                RemoteLog remoteLog = new RemoteLog
                    {
                        FanObjectNum = fanObjectId,
                        Date = DateTime.Now,
                        Person = "Диспетчер",
                        RemoteStateId = (int)dataForSending,
                        Sent = false
                    };
                repoUnit.RemoteLog.Add(remoteLog);
                repoUnit.RemoteLog.SaveChanges();
            }
        }
        public static RemouteFanState GetData(int fanObjectId)
        {
            RemoteLog remoteLog;
            try
            {
                using (var repoUnit = new RepoUnit())
                {
                    remoteLog = repoUnit.RemoteLog.LastRecord(f => f.FanObjectNum == fanObjectId);
                }
            }
            catch (Exception)
            {
                return RemouteFanState.Init;
            } 
            if (remoteLog.Sent)
                return RemouteFanState.Init;
            using (var repoUnit = new RepoUnit())
            {
                remoteLog.Sent = true;
                repoUnit.RemoteLog.Edit(remoteLog);
                repoUnit.RemoteLog.SaveChanges();
            }
            return (RemouteFanState) remoteLog.RemoteStateId;
        }
        /*public static void SetData(int fanObjectId, string parameter)
        {
            DataForSending[fanObjectId].Data = (RemouteFanState)Enum.Parse(typeof(RemouteFanState), parameter);
        }*/
    }
}
