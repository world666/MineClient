using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using DataRepository.DataAccess.GenericRepository;

namespace DataRepository.Models
{
    public class DoorsLog : IEntityId
    {
        public int Id { get; set; }

        public int DoorTypeId { get; set; }
        public int DoorStateId { get; set; }
        public int FanLogId { get; set; }

        public virtual DoorType DoorType { get; set; }
        public virtual DoorState DoorState { get; set; }
        public virtual FanLog FanLog { get; set; }
    }
}
