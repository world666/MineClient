using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataRepository.Models
{
    public class AnalogSignalLog
    {
        public int Id { get; set; }

        public int FanLogId { get; set; }
        public int SignalTypeId { get; set; }

        public int SignalValue { get; set; }

        public virtual FanLog FanLog { get; set; }
        public virtual AnalogSignal SignalType { get; set; }
    }
}
