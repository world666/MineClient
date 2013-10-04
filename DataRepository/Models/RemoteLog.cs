using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using DataRepository.DataAccess.GenericRepository;

namespace DataRepository.Models
{
    public class RemoteLog : IEntityId
    {
        public int Id { get; set; }
        public int FanObjectNum { get; set; }
        public DateTime Date { get; set; }
        public string Person { get; set; }
         [ForeignKey("RemoteState")]
        public int RemoteStateId { get; set; }
        public bool Sent { get; set; }

        public virtual RemoteState RemoteState { get; set; }
    }
}
