using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataRepository.DataAccess.GenericRepository;

namespace DataRepository.Models
{
    public class DoorType : IEntityId 
    {
        public int Id { get; set; }

        public string Type { get; set; }
    }
}
