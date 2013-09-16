using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace WpfClient.Model.Settings
{
    class FanObjectConfigData:ViewModelBase
    {
        private string _fanName;
        public string FanName
        {
            get { return _fanName; }
            set { _fanName = value; RaisePropertyChanged(FanName); }
        }
    }
}
