using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using DataRepository.DataAccess.GenericRepository;

namespace DataRepository.Models
{
    public class FanLog : IEntityId
    {
        public int Id { get; set; }

        public int FanNumber { get; set; }

        [ForeignKey("Fan1State")]
        public int? Fan1State_Id { get; set; }

        [ForeignKey("Fan2State")]
        public int? Fan2State_Id { get; set; }
        

        public DateTime Date { get; set; }

        public virtual FanState Fan1State { get; set; }
        public virtual FanState Fan2State { get; set; }
        public virtual ICollection<DoorsLog> DoorsLogs { get; set; }
        public virtual ICollection<AnalogSignalLog> AnalogSignalLogs { get; set; }
    }
}
