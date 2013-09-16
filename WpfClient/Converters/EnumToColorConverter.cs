using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using WpfClient.Model;
using WpfClient.Model.Entities;

namespace WpfClient.Converters
{
    public class EnumToColorConverter : IValueConverter 
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var v = (StateEnum)value;
            switch (v)
            {
                case StateEnum.Ok:
                    return new SolidColorBrush(Colors.LightGreen);
                case StateEnum.Warning:
                    return new SolidColorBrush(Colors.Orange);
                case StateEnum.Dangerous:
                    return new SolidColorBrush(Colors.IndianRed);
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
