using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class ThermometerControl : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty LevelProperty = DependencyProperty.Register("Level", typeof(double), typeof(ThermometerControl));

        public static readonly DependencyProperty MaxProperty = DependencyProperty.Register("Max", typeof(double), typeof(ThermometerControl), new PropertyMetadata(0.0, OnStartPropertyChanged));

        private static void OnStartPropertyChanged(DependencyObject dependencyObject,
                                                   DependencyPropertyChangedEventArgs e)
        {
            ThermometerControl thermometerControl = dependencyObject as ThermometerControl;
            for (int i = (int)thermometerControl.Max / 15; i >= 0; i--)
                thermometerControl.Lines.Add(i * 15);
        }

        private double _level;

        private double _max;

        public ObservableCollection<int> Lines { get; set; }

        public double Level
        {
            get { return _level; }
            set
            {
                _level = value > 1.0 ? 1.0 : value;
                OnPropertyChanged();
            }
        }

        public double Max
        {
            get { return (double)GetValue(MaxProperty); }
            set
            {
                SetValue(MaxProperty, value);
                OnPropertyChanged();
            }
        }

        public ThermometerControl()
        {
            Lines = new ObservableCollection<int>();

                InitializeComponent();
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
