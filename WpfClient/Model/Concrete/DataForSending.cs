using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfClient.Model.Concrete
{
    public class DataForSending
    {
        public int FanNum { get; set; }
        public RemouteFanState Data { get; set; }
    }
}
