using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Mc.CustomControls.Properties;

namespace Mc.CustomControls.Controls
{
    /// <summary>
    /// Interaction logic for IndicatorControl.xaml
    /// </summary>
    public partial class IndicatorControl : UserControl, INotifyPropertyChanged
    {
        private double _value;
        private double _level;

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(double), typeof(IndicatorControl));
        public static readonly DependencyProperty LevelProperty = DependencyProperty.Register("Level", typeof(double), typeof(IndicatorControl));

        public double Value {
            get { return _value; }
            set {
                _value = value;
                OnPropertyChanged();
            }
        }

        public double Level {
            get { return _level; }
            set {
                _level = value > 1.0 ? 1.0 : value;
                OnPropertyChanged();
            }
        }


        public IndicatorControl() {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
