using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace teammy
{
    public class BoolToMouseOverConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? "#99ADD8E6" : "#00000000";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
