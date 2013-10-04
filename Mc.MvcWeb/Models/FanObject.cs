using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mc.MvcWeb.Models
{
    public class FanObject
    {
        private List<Parameter> _parameters;
        private List<Door> _doors;

        public int FanObjectId { get; set; }

        public int WorkingFanNumber { get; set; }

        public List<Parameter> Parameters
        {
            get { return _parameters ?? (_parameters = new List<Parameter>()); }
        }

        public List<Door> Doors
        {
            get { return _doors ?? (_doors = new List<Door>()); }
        }

        public DateTime Date { get; set; }
    }
}