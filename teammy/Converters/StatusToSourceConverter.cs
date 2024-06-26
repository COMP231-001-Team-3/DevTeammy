﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

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
                    return "../images/notstarted.png";
                case "IP":
                    return "../images/progressIcon.jpg";
                case "CO":
                    return "../images/complete.png";
                default:
                    return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
