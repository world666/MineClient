using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mc.MvcWeb.Models.Control
{
    public class RemoteData
    {
        public string Password { get; set; }
        public int FanObjectId { get; set; }
        public int Command { get; set; }
    }
}