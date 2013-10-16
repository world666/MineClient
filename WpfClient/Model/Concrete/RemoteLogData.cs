using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfClient.Model.Concrete
{
    public class RemoteLogData
    {
        public int Id;
        public DateTime Date { get; set; }
        public int FanObjectNum { get; set; }
        public string Person { get; set; }
        public string RemoteCommand { get; set; }
        public string State { get; set; }
        public string DateFormat
        {
            get { return Date.ToString("dd.mm.yyyy hh:mm:ss"); }
        }
    }
}
