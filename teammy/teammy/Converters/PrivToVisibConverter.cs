using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace teammy
{
    public class PrivToVisibConverter : IValueConverter
    {
        /// <summary>
        ///     Converts user privilege into visibility for create button
        /// </summary>
        /// <returns>The Visibility string</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value as user).privilege_code.Equals("PM") ? "Visible" : "Hidden";
        }
        /// <summary>
        ///     Present here only for the interface implementation
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
