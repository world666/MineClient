using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace Mc.CustomControls.Controls
{
    /// <summary>
    /// Interaction logic for VerticalPipe.xaml
    /// </summary>
    public partial class VerticalPipe : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty MoveProperty =
                    DependencyProperty.Register("Move", typeof(string), typeof(VerticalPipe),
                    new PropertyMetadata("", OnStartPropertyChanged));

        private static void OnStartPropertyChanged(DependencyObject dependencyObject,
                                                   DependencyPropertyChangedEventArgs e) {
            var pipe = ((VerticalPipe)dependencyObject);
            if (pipe.Move == "Down") {
                pipe.image_top.Visibility = Visibility.Hidden;
                pipe.image_down.Visibility = Visibility.Visible;
            } else if (pipe.Move == "Stop") {
                pipe.image_down.Visibility = Visibility.Hidden;
                pipe.image_top.Visibility = Visibility.Hidden;
            } else if (pipe.Move == "Top") {
                pipe.image_down.Visibility = Visibility.Hidden;
                pipe.image_top.Visibility = Visibility.Visible;
            }
        }

        public string Move {
            get {
                return (string)GetValue(MoveProperty);
            }
            set {
                SetValue(MoveProperty, value);
            }
        }
        private Color _pipeColor;
        public Color PipeColor {
            get { return _pipeColor; }
            set { _pipeColor = value; OnPropertyChanged("PipeColor"); }
        }
        public VerticalPipe() {
            InitializeComponent();
            Loaded += DrawLine;
        }
        void DrawLine(Object sender, RoutedEventArgs e) {
            try {
                BitmapImage src_down = new BitmapImage();
                src_down.BeginInit();
                src_down.UriSource = new Uri(@"../../Images/img1.png", UriKind.Relative);
                src_down.Rotation = Rotation.Rotate90;
                src_down.CacheOption = BitmapCacheOption.OnLoad;
                src_down.EndInit();
                image_down.Source = src_down;

                BitmapImage src_top = new BitmapImage();
                src_top.BeginInit();
                src_top.UriSource = new Uri(@"../../Images/img1.png", UriKind.Relative);
                src_top.Rotation = Rotation.Rotate270;
                src_top.CacheOption = BitmapCacheOption.OnLoad;
                src_top.EndInit();
                image_top.Source = src_top;



                TranslateTransform trans_down = new TranslateTransform();
                image_down.RenderTransform = trans_down;
                DoubleAnimation anim_down = new DoubleAnimation(-20, canvas.ActualHeight, TimeSpan.FromMilliseconds(4000));
                anim_down.RepeatBehavior = RepeatBehavior.Forever;
                trans_down.BeginAnimation(TranslateTransform.YProperty, anim_down);

                TranslateTransform trans_top = new TranslateTransform();
                image_top.RenderTransform = trans_top;
                DoubleAnimation anim_top = new DoubleAnimation(20, -canvas.ActualHeight, TimeSpan.FromMilliseconds(4000));
                anim_top.RepeatBehavior = RepeatBehavior.Forever;
                trans_top.BeginAnimation(TranslateTransform.YProperty, anim_top);
            } catch (Exception ex) { }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
