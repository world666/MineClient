using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfClient.Model.Entities
{
    public class Door
    {
        public string Type { get; set; }
        public string State { get; set; }

        public int StateId { get; set; }
        public int TypeId { get; set; }
    }
}
