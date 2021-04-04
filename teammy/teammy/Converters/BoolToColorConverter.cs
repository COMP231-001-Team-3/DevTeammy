using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace teammy
{
    public class BoolToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? "#FF7560BD" : parameter == null ? "#FFB9A4FF" : string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
