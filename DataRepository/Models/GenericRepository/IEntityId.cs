using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataRepository.DataAccess.GenericRepository
{
    public interface IEntityId
    {
        int Id { get; set; }
    }
}
