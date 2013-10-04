using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataRepository.DataAccess.GenericRepository;

namespace DataRepository.Models
{
    public class SettingsLog : IEntityId 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double DValue { get; set; }
        public double Warning { get; set; }
        public double Danger { get; set; }
        public string SValue { get; set; }
    }
}
