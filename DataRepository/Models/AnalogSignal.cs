using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataRepository.DataAccess.GenericRepository;

namespace DataRepository.Models
{
    public class AnalogSignal : IEntityId 
    {
        public int Id { get; set; }

        public string Type { get; set; }
    }
}
