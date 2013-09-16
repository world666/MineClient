using System;
using System.Timers;

namespace WpfClient.Model
{
    public static class AsyncProvider
    {
        public static void StartTimer(int milliSeconds, Action action)
        {
            var timer = new Timer(milliSeconds);
            timer.Elapsed += (sender, args) => action(); 
            timer.Start();
            timer.Enabled = true;
        }

    }
}
