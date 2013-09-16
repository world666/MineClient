using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using DataRepository.DataAccess.GenericRepository;
using DataRepository.Models;

namespace DataRepository.DataAccess.GenericRepository
{
    public class RepoUnit : IDisposable
    {
        private MineContext _context;
        private IDataRepository<FanLog> _fanLog;
        private IDataRepository<AnalogSignal> _analogSignal;
        private IDataRepository<DoorType> _doorType; 

        private MineContext getContext()
        {
            return _context ?? (_context = new MineContext());
        }


        public IDataRepository<FanLog> FanLog 
        {
            get { return _fanLog ?? (_fanLog = getRepository<FanLog>()); }
        }

        public IDataRepository<AnalogSignal> AnalogSignal
        {
            get { return _analogSignal ?? (_analogSignal = getRepository<AnalogSignal>()); }
        }

        public IDataRepository<DoorType> DoorType
        {
            get { return _doorType ?? (_doorType = getRepository<DoorType>()); }
        }

        public void Commit() {
            try {
                _context.SaveChanges();
            } catch (DbEntityValidationException dbEx) {
                foreach (var validationErrors in dbEx.EntityValidationErrors) {
                    foreach (var validationError in validationErrors.ValidationErrors) {
                        Console.WriteLine("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            } catch (Exception ex) {
                Console.WriteLine("Save failed", ex);
            }
        }

        public void Dispose() {
            if (_context != null)
                _context.Dispose();
            
            //GC.SuppressFinalize(this);
        }

        private IDataRepository<T> getRepository<T>()
            where T : class, IEntityId
        {
            return new DataRepository<T>(getContext());
        }
    }
}
