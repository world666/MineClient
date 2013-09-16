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
    /// Interaction logic for HorizontalPipe.xaml
    /// </summary>
    public partial class HorizontalPipe : UserControl, INotifyPropertyChanged
    {
        private Color _pipeColor;
        public Color PipeColor {
            get { return _pipeColor; }
            set { _pipeColor = value; OnPropertyChanged("PipeColor"); }

        }
        public static readonly DependencyProperty MoveProperty =
                    DependencyProperty.Register("Move", typeof(string), typeof(HorizontalPipe),
                    new PropertyMetadata("", OnStartPropertyChanged));

        private static void OnStartPropertyChanged(DependencyObject dependencyObject,
                                                   DependencyPropertyChangedEventArgs e) {
            var pipe = ((HorizontalPipe)dependencyObject);
            if (pipe.Move == "Reight") {
                pipe.image_left.Visibility = Visibility.Hidden;
                pipe.image_reight.Visibility = Visibility.Visible;
            } else if (pipe.Move == "Stop") {
                pipe.image_reight.Visibility = Visibility.Hidden;
                pipe.image_left.Visibility = Visibility.Hidden;
            } else if (pipe.Move == "Left") {
                pipe.image_reight.Visibility = Visibility.Hidden;
                pipe.image_left.Visibility = Visibility.Visible;
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

        public HorizontalPipe() {
            InitializeComponent();
            // Add a Line Element
            Loaded += DrawLine;

        }

        void DrawLine(Object sender, RoutedEventArgs e) {
            try {
                BitmapImage src_reight = new BitmapImage();
                src_reight.BeginInit();
                src_reight.UriSource = new Uri(@"../../Images/img1.png", UriKind.Relative);
                //src_reight.UriSource = new Uri(@"pack://application:,,,/Images/img1.png", UriKind.Absolute);
                src_reight.CacheOption = BitmapCacheOption.OnLoad;
                src_reight.EndInit();
                image_reight.Source = src_reight;

                BitmapImage src_left = new BitmapImage();
                src_left.BeginInit();
                src_left.UriSource = new Uri(@"../../Images/img1.png", UriKind.Relative);
                //src_left.UriSource = new Uri(@"pack://application:,,,/Images/img1.png", UriKind.Absolute);
                src_left.Rotation = Rotation.Rotate180;
                src_left.CacheOption = BitmapCacheOption.OnLoad;
                src_left.EndInit();
                image_left.Source = src_left;



                TranslateTransform trans_reight = new TranslateTransform();
                image_reight.RenderTransform = trans_reight;
                DoubleAnimation anim_reight = new DoubleAnimation(-20, canvas.ActualWidth, TimeSpan.FromMilliseconds(4000));
                anim_reight.RepeatBehavior = RepeatBehavior.Forever;
                trans_reight.BeginAnimation(TranslateTransform.XProperty, anim_reight);

                TranslateTransform trans_left = new TranslateTransform();
                image_left.RenderTransform = trans_left;
                DoubleAnimation anim_left = new DoubleAnimation(20, -canvas.ActualWidth, TimeSpan.FromMilliseconds(4000));
                anim_left.RepeatBehavior = RepeatBehavior.Forever;
                trans_left.BeginAnimation(TranslateTransform.XProperty, anim_left);
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
