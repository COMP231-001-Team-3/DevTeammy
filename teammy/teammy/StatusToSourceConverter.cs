using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace teammy
{
    public class StatusToSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string status = value != null ? value.ToString() : null;
            switch (status)
            {
                case "NS":
                    return "images/notstarted.png";
                case "IP":
                    return "images/progressIcon.jpg";
                case "CO":
                    return "images/complete.png";
                default:
                    return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
