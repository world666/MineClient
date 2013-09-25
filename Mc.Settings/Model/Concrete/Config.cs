using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mc.Settings.Model.Abstract;
using Mc.Settings.Services;

namespace Mc.Settings.Model.Concrete
{
    public class Config
    {
        private static readonly IConfig _config = new MineConfig();

        public static IConfig Instance
        {
            get { return _config; }
        }
    }
}
