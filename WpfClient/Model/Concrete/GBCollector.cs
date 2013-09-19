using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfClient.Model.Concrete
{
    static class GBCollector
    {
        public static void StartCollection()
        {
            //AsyncProvider.StartTimer(10000, Collect);

        }
        private static void Collect()
        {
            GC.Collect();
            GC.WaitForFullGCComplete();
        }
    }
}
