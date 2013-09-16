using System.Data.Entity;
using DataRepository.Models;

namespace DataRepository.DataAccess
{
    public class MineContext : DbContext
    {
        public IDbSet<FanLog> FanLog { get; set; }
        public IDbSet<DoorsLog> DoorsLog { get; set; }
        
        public IDbSet<DoorType> DoorsType { get; set; }
        public IDbSet<AnalogSignal> AnalogSignals { get; set; } 

        public IDbSet<DoorState> DoorState { get; set; }
        public IDbSet<FanState> FanState { get; set; }

        public MineContext()
            : base(GetConnectionName())
        {
            Configuration.LazyLoadingEnabled = true;
        }

        protected static string GetConnectionName() {
            return @"Data Source=.\SQLExpress;Database=MineDb;Trusted_Connection=True;";
            //return @"Data Source=(localdb)\Projects;Database=MineDb;Trusted_Connection=True;";

        }
    }
}
