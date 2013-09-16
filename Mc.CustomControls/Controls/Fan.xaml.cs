using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Mc.CustomControls.Controls
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class Fan : UserControl, INotifyPropertyChanged
    {
        private RotateTransform rt;
        private DoubleAnimation da;
        public static readonly DependencyProperty RotationProperty =
                    DependencyProperty.Register("Rotation", typeof(bool), typeof(Fan),
                    new PropertyMetadata(false, OnStartPropertyChanged));

        private static void OnStartPropertyChanged(DependencyObject dependencyObject,
                                                   DependencyPropertyChangedEventArgs e)
        {
            var fn = ((Fan) dependencyObject);
            if (fn.Rotation)
            {
                fn.rt.BeginAnimation(RotateTransform.AngleProperty, fn.da);
                fn.FanControl.Background = Brushes.AliceBlue;
            }
            else
            {
                fn.rt.BeginAnimation(RotateTransform.AngleProperty, null);
                fn.FanControl.Background = Brushes.Pink;
            }
        }

        public bool Rotation
        {
            get
            {
                return (bool)GetValue(RotationProperty); 
            }
            set
            {
                SetValue(RotationProperty, value);  
            }
        }

        private double _radius;
        public double Radius
        {
            get { return _radius; }
            set { _radius = value; OnPropertyChanged("Radius");
                WedgeAngle = _radius*10/5;
                FanControl.Width = value*2 + 5;
                FanControl.Height = value*2 + 5;
            }
        }

        private double _wedgeAngle;
        public double WedgeAngle
        {
            get { return _wedgeAngle; }
            set { _wedgeAngle = value; OnPropertyChanged("WedgeAngle"); }
        }
        public Fan()
        {
            InitializeComponent();
            da = new DoubleAnimation(360, 0, new Duration(TimeSpan.FromMilliseconds(2000)));
            rt = new RotateTransform();
            CanvasFan.RenderTransform = rt;
            CanvasFan.RenderTransformOrigin = new Point(0.5, 0.5);
            da.RepeatBehavior = RepeatBehavior.Forever;
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
