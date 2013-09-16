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

        private double _level;

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

        public ThermometerControl()
        {
            Lines = new ObservableCollection<int>();
            for (int i = 8; i >= 0; i-- )
                Lines.Add(i*15);

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
