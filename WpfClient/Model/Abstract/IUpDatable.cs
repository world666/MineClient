using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataRepository.Models;

namespace WpfClient.Model.Abstract
{
    public interface IUpDatable
    {
        void Update(FanLog fanLog);
    }
}
