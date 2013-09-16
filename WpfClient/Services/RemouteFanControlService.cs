using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClient.Model;
using WpfClient.Model.Concrete;

namespace WpfClient.Services
{
    public static class RemouteFanControlService
    {
        public static List<DataForSending> DataForSending { get; private set; }
        static RemouteFanControlService()
        {
            DataForSending = new List<DataForSending>();
            for (int i = 0; i < 10; i++)
            {
                DataForSending.Add(new DataForSending { Data = RemouteFanState.Init, FanNum = i });
            }
        }
        public static void SetData(int fanObjectId, RemouteFanState dataForSending)
        {
            DataForSending[fanObjectId].Data = dataForSending;
        }
        public static void SetData(int fanObjectId, string parameter)
        {
            DataForSending[fanObjectId].Data = (RemouteFanState)Enum.Parse(typeof(RemouteFanState), parameter);
        }
    }
}
