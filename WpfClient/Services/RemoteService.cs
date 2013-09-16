using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using WpfClient.Model;
using WpfClient.Model.Abstract;
using WpfClient.Model.Concrete;

namespace WpfClient.Services
{
    public static class RemoteService
    {
        public static List<RemoteData> LastRecieve { get; private set; }

        static RemoteService()
        {
            LastRecieve = new List<RemoteData>();
        }
        public static void onRecieve(string str)
        {
            var parameters = IoC.Resolve<IMsgParser>().Parse(str);
            
            int fanObjectNum = parameters["fn"];

            if (fanObjectNum  > LastRecieve.Count)
                LastRecieve.Add(new RemoteData {FanObjectId = fanObjectNum, LastRecieve = DateTime.Now});
            else
                LastRecieve.FirstOrDefault(f => f.FanObjectId == fanObjectNum).LastRecieve = DateTime.Now;
        }
        public static DateTime GetLastRecieve(int fanNum)
        {
            try
            {
                DateTime dateTime = LastRecieve.FirstOrDefault(f => f.FanObjectId == fanNum).LastRecieve;
                return dateTime;
            }
            catch (Exception)
            {
                return System.DateTime.Now - new TimeSpan(24, 0, 0);
            }
        }
    }
}
