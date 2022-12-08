using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace teammy
{
    public class PriorityToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value != null)
            {
                switch (value.ToString())
                {
                    case "High":
                        return "Red";
                    case "Medium":
                        return "Yellow";
                    case "Low":
                        return "Blue";
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
