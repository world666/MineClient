using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using Mc.CustomControls.Properties;

namespace Mc.CustomControls.Controls
{
    /// <summary>
    /// Interaction logic for IndicatorView.xaml
    /// </summary>
    public partial class IndicatorControl : UserControl, INotifyPropertyChanged
    {
        private double _value;
        private double _level;

        public double Value
        {
            get { return _value; }
            set
            {
                _value = value;
                OnPropertyChanged();
            }
        }

        public double Level
        {
            get { return _level; }
            set 
            {
                _level = value > 1.0 ? 1.0 : value; 
                OnPropertyChanged();
            }
        }
        

        public IndicatorControl() {
            InitializeComponent();
            Level = 0.5;
            Value = 100.5;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
