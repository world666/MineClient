using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using GalaSoft.MvvmLight;
using WpfClient.Model;

namespace WpfClient.ViewModel
{
    public class DateTimeVm : ViewModelBase
    {
        private string _date;
        private string _time;

        public DateTimeVm()
        {
            setDateTime();
            
            var timer = new DispatcherTimer(DispatcherPriority.Normal);
            timer.Interval = new TimeSpan(0,0,0,1);
            timer.Tick += (sender, args) => setDateTime();
            timer.Start();
        }

        public string Date
        {
            get { return _date; }
            set
            {
                _date = value;
                RaisePropertyChanged("Date");
            }
        }

        public string Time 
        {
            get { return _time; }
            set
            {
                _time = value;
                RaisePropertyChanged("Time");
            }
        }

        private void setDateTime()
        {
            var dateTime = DateTime.Now;
            Date = dateTime.ToShortDateString();
            Time = dateTime.ToLongTimeString();
        }
    }
}
