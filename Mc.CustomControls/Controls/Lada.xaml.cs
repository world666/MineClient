using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Mc.CustomControls.Model;

namespace Mc.CustomControls.Controls
{
    /// <summary>
    /// Interaction logic for Lada.xaml
    /// </summary>
    public partial class Lada : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty StateProperty =
                    DependencyProperty.Register("State", typeof(DoorStateEnum), typeof(Lada),
                    new PropertyMetadata(DoorStateEnum.Close, OnStartPropertyChanged));

        private static void OnStartPropertyChanged(DependencyObject dependencyObject,
                                                   DependencyPropertyChangedEventArgs e) {
            var lada = ((Lada)dependencyObject);
            if (lada.State == DoorStateEnum.Open) {
                lada.Close.Visibility = Visibility.Hidden;
                lada.Open.Visibility = Visibility.Visible;
            } else if (lada.State == DoorStateEnum.Close) {
                lada.Open.Visibility = Visibility.Hidden;
                lada.Close.Visibility = Visibility.Visible;
            } else if (lada.State == DoorStateEnum.Undefined) {
                lada.Open.Visibility = Visibility.Hidden;
                lada.Close.Visibility = Visibility.Hidden;
            }
        }
        public DoorStateEnum State {
            get {
                return (DoorStateEnum)GetValue(StateProperty);
            }
            set {
                SetValue(StateProperty, value);
            }
        }

        //private Color _openColor;
        private Color _openColor;
        public Color OpenColor
        {
            get { return _openColor; }
            set { _openColor = value; OnPropertyChanged("OpenColor"); }
        }
        private Color _closeColor;
        public Color CloseColor
        {
            get { return _closeColor; }
            set { _closeColor = value; OnPropertyChanged("CloseColor"); }
        }
        public Lada() {
            InitializeComponent();
            Open.Visibility = Visibility.Hidden;
            Close.Visibility = Visibility.Visible;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
